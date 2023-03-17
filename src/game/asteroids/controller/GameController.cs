using Worms.engine.camera;
using Worms.engine.core.input;
using Worms.engine.core.input.listener;
using Worms.engine.core.window;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.audio_source;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.scripts;
using Worms.engine.helper;
using Worms.game.asteroids.asteroids;
using Worms.game.asteroids.names;
using Worms.game.asteroids.player;
using Worms.game.asteroids.saucer;

namespace Worms.game.asteroids.controller; 

public class GameController : Script {
    private const float MIN_SAUCER_SPAWN_TIME = 10;
    private const float MAX_SAUCER_SPAWN_TIME = 30;
    private const float FAR_AWAY = 10000;
    private const float PLAY_AREA_BORDER = 100f;

    private Transform _enemyHolder = null!;
    private Transform _lifeDisplayHolder = null!;
    private AudioSource _lifeAudioSource = null!;
    private MusicScript _musicScript = null!;
    private Vector2 _playArea;
    private List<PolygonCollider> _colliders = null!;
    private Transform _player = null!;

    private bool _respawnPlayer = false;
    private bool _waveOver = true;
    private readonly ClockTimer _spawnAsteroidsTimer = new(1.5f, 1.5f);
    private readonly ClockTimer _respawnTimer = new(3);
    private readonly ClockTimer _saucerSpawnerTimer = new(MIN_SAUCER_SPAWN_TIME);
    private long _round = 1;

    private int Lives {
        get => _lives;
        set {
            foreach (Transform child in _lifeDisplayHolder.children) {
                child.gameObject.Destroy();
            }

            _lives = value;
            LifeFactory.Create(_lifeDisplayHolder, _lives);
        }
    }

    private int _lives;
    
    public GameController() {
        PlayerBase.PlayerDieEvent += PlayerDied;
        _saucerSpawnerTimer.Duration = RandomUtil.GetRandomValueBetweenTwoValues(MIN_SAUCER_SPAWN_TIME, MAX_SAUCER_SPAWN_TIME);
    }

    public override void Awake() {
        _lifeAudioSource = GetComponent<AudioSource>();
        _enemyHolder = Transform.Instantiate(GameObjectBuilder.Builder("enemyHolder")).Transform;
        _colliders = GetComponents<PolygonCollider>();
        Camera.Main.Size = 1.5f;
        CalculateScreenArea(WindowManager.CurrentResolution);
        _lifeDisplayHolder = Transform.Instantiate(GameObjectBuilder
            .Builder("lifeHolder")
            .SetPosition(new Vector2(-WindowManager.CurrentResolution.x / 2f + 20, WindowManager.CurrentResolution.y / 2f - 25) * Camera.Main.Size)
        ).Transform;
        Lives = 3;
    }

    public override void Start() {
        _musicScript = Transform.GetRoot().GetComponentInChildren<MusicScript>();
        SpawnPlayer();
    }

    public override void Update(float deltaTime) {
        if (Input.GetKeyDown(Button.B)) {
            foreach (Transform child in _enemyHolder.children) {
                child.gameObject.Destroy();
            }
        }

        HandleSaucerSpawning(deltaTime);
        HandlePlayerRespawn(deltaTime);
        if (_enemyHolder.children.Count == 0) {
            HandleWaveSpawn(deltaTime);
        }
    }

    private void HandleSaucerSpawning(float deltaTime) {
        _saucerSpawnerTimer.Time += deltaTime;
        if (_saucerSpawnerTimer.Expired()) {
            _saucerSpawnerTimer.Reset();
            _saucerSpawnerTimer.Duration = RandomUtil.GetRandomValueBetweenTwoValues(MIN_SAUCER_SPAWN_TIME, MAX_SAUCER_SPAWN_TIME);
            bool random = RandomUtil.RandomBool();
            float skillRatio = Math.Min(0.5f + 0.05f * _round, 0.95f);
            SaucerSettings settings = new(_enemyHolder, GetSaucerSpawnPosition(), random ? () => _player : null, skillRatio);
            SaucerFactory.Create(settings);
        }
    }

    private Vector2 GetSaucerSpawnPosition() {
        int side = RandomUtil.RandomBool() ? 1 : -1;
        const float SAUCER_OFFSET = 50;
        float y = RandomUtil.GetRandomValueBetweenTwoValues(-_playArea.y / 2f, _playArea.y / 2f);
        return new Vector2(side * (_playArea.x / 2f) - SAUCER_OFFSET * side, y);
    }

    private void HandlePlayerRespawn(float deltaTime) {
        _respawnTimer.Time += deltaTime;
        if (_respawnPlayer && _respawnTimer.Expired()) {
            _respawnPlayer = false;
            SpawnPlayer();
        }
    }
    
    private void HandleWaveSpawn(float deltaTime) {
        if (!_waveOver) {
            _waveOver = true;
            _spawnAsteroidsTimer.Reset();
        }
        _spawnAsteroidsTimer.Time += deltaTime;
        if (_spawnAsteroidsTimer.Expired()) {
            if (_round % 3 == 0) {
                _lifeAudioSource.Play();
                Lives++;
            }
            SpawnAsteroidWave();
        }
    }
    
    private void SpawnAsteroidWave() {
        _musicScript.RestartMusic();
        _waveOver = false;
        long spawnAmount = Math.Min(2 + _round++, 100);
        for (int i = 0; i < spawnAmount; i++) {
            AsteroidFactory.Create(_enemyHolder, AsteroidType.BIG, GetRandomPositionAlongBorder());
        }
    }
    
    private Vector2 GetRandomPositionAlongBorder() {
        Vector2 position;
        float p = RandomUtil.GetRandomValueBetweenTwoValues(0, _playArea.x * 2 + _playArea.y * 2);
        if (p < _playArea.x + _playArea.y) {
            if (p < _playArea.x) {
                position.x = p;
                position.y = 0;
            }
            else {
                position.x = _playArea.x;
                position.y = p - _playArea.x;
            }
        }
        else {
            p -= _playArea.x + _playArea.y;
            if (p < _playArea.x) {
                position.x = _playArea.x - p;
                position.y = _playArea.y;
            }
            else {
                position.x = 0;
                position.y = _playArea.y - (p - _playArea.x);
            }
        }

        return (position - new Vector2(_playArea.x, _playArea.y) / 2f) * 1.5f;
    }

    
    public override void OnTriggerEnter(Collider collider) {
        Vector2 pos = collider.Transform.Parent!.Position;
        List<Vector2> corners = collider.GetLocalCorners()
            .Select(c => collider.Transform.LocalToWorldMatrix.ConvertPoint(c))
            .ToList();
        float maxY = Math.Abs(corners.MaxBy(c => Math.Abs(c.y)).y);
        float maxX = Math.Abs(corners.MaxBy(c => Math.Abs(c.x)).x);
        
        Vector2 half = _playArea / 2f;
        
        if (maxY > half.y) {
            pos = new Vector2(pos.x, -pos.y + Math.Sign(pos.y) * (maxY - half.y + 10));
        }

        if (maxX > _playArea.x / 2f) {
            if (collider.gameObject.Tag == TagNames.ENEMY) {
                collider.Transform.Parent!.gameObject.Destroy();
                return;
            }
            
            pos = new Vector2(-pos.x + Math.Sign(pos.x) * (maxX - half.x + 10), pos.y);
        }

        collider.Transform.Parent!.Position = pos;
    }

    private void CalculateScreenArea(Vector2Int resolution) {
        _playArea = new Vector2(resolution.x + PLAY_AREA_BORDER, resolution.y + PLAY_AREA_BORDER) * Camera.Main.Size;
        float minX = -_playArea.x / 2;
        float maxX = _playArea.x / 2;
        float minY = -_playArea.y / 2;
        float maxY = _playArea.y / 2;
        _colliders[0].Vertices = new Vector2[] { new(-FAR_AWAY, minY), new(-FAR_AWAY, maxY), new(minX, maxY), new(minX, minY) };
        _colliders[1].Vertices = new Vector2[] { new(-FAR_AWAY, maxY), new(-FAR_AWAY, FAR_AWAY), new(FAR_AWAY, FAR_AWAY), new(FAR_AWAY, maxY) };
        _colliders[2].Vertices = new Vector2[] { new(maxX, maxY), new(FAR_AWAY, maxY), new(FAR_AWAY, minY), new(maxX, minY) };
        _colliders[3].Vertices = new Vector2[] { new(FAR_AWAY, minY), new(FAR_AWAY, -FAR_AWAY), new(-FAR_AWAY, -FAR_AWAY), new(-FAR_AWAY, minY) };
    }

    private void PlayerDied() {
        Lives--;
        if (Lives != 0) {
            _respawnPlayer = true;
            _respawnTimer.Reset();
            return;
            
        }
        Console.WriteLine("dead");
    }
    
    private void SpawnPlayer() {
        _player = PlayerFactory.Create(Transform.GetRoot());
    }
}