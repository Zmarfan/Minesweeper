using Worms.engine.camera;
using Worms.engine.core.window;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.scripts;
using Worms.game.asteroids.asteroids;
using Worms.game.asteroids.names;
using Worms.game.asteroids.player;
using Worms.game.asteroids.saucer;

namespace Worms.game.asteroids.camera; 

public class PlayArea : Script {
    private const float PLAY_AREA_BORDER = 15f;

    private BoxCollider _boxCollider = null!;
    private Transform _player = null!;
    
    public PlayArea() : base(true) {
        PlayerMovement.PlayerDieEvent += SpawnPlayer;
    }

    public override void Awake() {
        _boxCollider = GetComponent<BoxCollider>();
        
        Camera.Main.Size = 2f;
        ResolutionChanged(WindowManager.CurrentResolution);
        WindowManager.ResolutionChangedEvent += ResolutionChanged;
    }

    public override void Start() {
        SpawnPlayer();
        SaucerSettings settings = new(Transform.GetRoot(), new Vector2(1200, 0), _player, false, 0f);
        SaucerFactory.Create(settings);
        for (int i = 0; i < 10; i++) {
            AsteroidFactory.Create(Transform.GetRoot(), AsteroidType.BIG, new Vector2(-1200, 0));
        }
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

    private void SpawnPlayer() {
        _player = PlayerFactory.Create(Transform.GetRoot());
    }
}