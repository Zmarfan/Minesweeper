using Worms.engine.core.window;
using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.screen_pivot; 

public class ScreenPivot : Script {
    private readonly Vector2Int _pivot;
    private Vector2Int _oldResolution;
    
    public ScreenPivot(Vector2Int pivot, bool isActive, string name = "pivot") : base(isActive, name) {
        _pivot = pivot;
    }

    public override void Awake() {
        SetPivot(WindowManager.CurrentResolution);
        _oldResolution = WindowManager.CurrentResolution;
    }

    public override void Update(float deltaTime) {
        if (WindowManager.CurrentResolution != _oldResolution) {
            SetPivot(WindowManager.CurrentResolution);
            _oldResolution = WindowManager.CurrentResolution;
        }
    }

    private void SetPivot(Vector2Int resolution) {
        Vector2Int position = resolution / -2 + resolution * _pivot;
        Transform.Position = new Vector2(position.x, position.y);
    }
}