using Worms.engine.camera;
using Worms.engine.core.window;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.scripts;
using Worms.engine.helper;
using Worms.game.asteroids.asteroids;
using Worms.game.asteroids.names;
using Worms.game.asteroids.player;
using Worms.game.asteroids.saucer;

namespace Worms.game.asteroids.controller; 

public class GameController : Script {
    private const float PLAY_AREA_BORDER = 15f;

    private BoxCollider _boxCollider = null!;
    private Transform _player = null!;

    private bool _respawnPlayer = false;
    private ClockTimer _respawnTimer = new(3);
    private long _round = 0;
    
    public GameController() {
        PlayerBase.PlayerDieEvent += PlayerDied;
    }

    public override void Awake() {
        _boxCollider = GetComponent<BoxCollider>();
        
        Camera.Main.Size = 2f;
        ResolutionChanged(WindowManager.CurrentResolution);
        WindowManager.ResolutionChangedEvent += ResolutionChanged;
    }

    public override void Start() {
        SpawnPlayer();
    }

    public override void Update(float deltaTime) {
        HandlePlayerRespawn(deltaTime);

        if (AllEnemiesCleared()) {
            SpawnAsteroidWave();
        }
    }

    private bool AllEnemiesCleared() {
        return false; // count children, also make it possible to have more than one collider as trigger/collider
    }

    private void HandlePlayerRespawn(float deltaTime) {
        _respawnTimer.Time += deltaTime;
        if (_respawnPlayer && _respawnTimer.Expired()) {
            _respawnPlayer = false;
            SpawnPlayer();
        }
    }
    
    private void SpawnAsteroidWave() {
        long spawnAmount = 3 + _round++;
        for (int i = 0; i < spawnAmount; i++) {
            AsteroidFactory.Create(Transform.GetRoot(), AsteroidType.BIG, GetRandomPositionAlongBorder());
        }
    }
    
    private Vector2 GetRandomPositionAlongBorder() {
        Vector2 position;
        float p = RandomUtil.GetRandomValueBetweenTwoValues(0, _boxCollider.size.x * 2 + _boxCollider.size.y * 2);
        if (p < _boxCollider.size.x + _boxCollider.size.y) {
            if (p < _boxCollider.size.x) {
                position.x = p;
                position.y = 0;
            }
            else {
                position.x = _boxCollider.size.x;
                position.y = p - _boxCollider.size.x;
            }
        }
        else {
            p -= _boxCollider.size.x + _boxCollider.size.y;
            if (p < _boxCollider.size.x) {
                position.x = _boxCollider.size.x - p;
                position.y = _boxCollider.size.y;
            }
            else {
                position.x = 0;
                position.y = _boxCollider.size.y - (p - _boxCollider.size.x);
            }
        }

        return position - new Vector2(_boxCollider.size.x / 2f, _boxCollider.size.y / 2f);
    }

    
    public override void OnTriggerExit(Collider collider) {
        Vector2 half = _boxCollider.size / 2;
        Transform transform = collider.Transform.Parent!;
        if (transform.Position.y < -half.y) {
            transform.Position = new Vector2(transform.Position.x, half.y);
        }
        if (transform.Position.y > half.y) {
            transform.Position = new Vector2(transform.Position.x, -half.y);
        }

        if (collider.gameObject.Tag == TagNames.ENEMY) {
            collider.Transform.Parent!.gameObject.Destroy();
            return;
        }
        
        if (transform.Position.x < -half.x) {
            transform.Position = new Vector2(half.x, transform.Position.y);
        }
        if (transform.Position.x > half.x) {
            transform.Position = new Vector2(-half.x, transform.Position.y);
        }
    }

    private void ResolutionChanged(Vector2Int resolution) {
        _boxCollider.size = new Vector2(resolution.x + PLAY_AREA_BORDER, resolution.y + PLAY_AREA_BORDER) * Camera.Main.Size;
    }

    private void PlayerDied() {
        _respawnPlayer = true;
        _respawnTimer.Reset();
    }
    
    private void SpawnPlayer() {
        _player = PlayerFactory.Create(Transform.GetRoot());
    }
}