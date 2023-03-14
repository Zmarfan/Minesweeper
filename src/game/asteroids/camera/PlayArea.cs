using Worms.engine.camera;
using Worms.engine.core.window;
using Worms.engine.data;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.scripts;

namespace Worms.game.asteroids.camera; 

public class PlayArea : Script {
    private BoxCollider _boxCollider = null!;
    
    public PlayArea() : base(true) {
    }

    public override void Awake() {
        _boxCollider = GetComponent<BoxCollider>();
        
        Camera.Main.Size = 2;
        ResolutionChanged(WindowManager.CurrentResolution);
        WindowManager.ResolutionChangedEvent += ResolutionChanged;
    }

    public override void OnTriggerExit(Collider collider) {
        Vector2 half = _boxCollider.size / 2;
        engine.game_object.Transform transform = collider.Transform.Parent!;
        if (transform.Position.x < -half.x) {
            transform.Position = new Vector2(half.x, transform.Position.y);
        }
        if (transform.Position.x > half.x) {
            transform.Position = new Vector2(-half.x, transform.Position.y);
        }
        if (transform.Position.y < -half.y) {
            transform.Position = new Vector2(transform.Position.x, half.y);
        }
        if (transform.Position.y > half.y) {
            transform.Position = new Vector2(transform.Position.x, -half.y);
        }
    }

    private void ResolutionChanged(Vector2Int resolution) {
        _boxCollider.size = new Vector2(resolution.x, resolution.y) * Camera.Main.Size;
    }
}