using GameEngine.engine.core.window;
using GameEngine.engine.data;
using GameEngine.engine.game_object.scripts;

namespace GameEngine.engine.game_object.components.screen_pivot; 

public class ScreenPivot : Script {
    private readonly Vector2 _pivot;
    private Vector2Int _oldResolution;
    
    public ScreenPivot(Vector2 pivot, bool isActive = true, string name = "pivot") : base(isActive, name) {
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
        Vector2 res = new(resolution.x, resolution.y);
        Vector2 position = res / -2 + res * _pivot;
        Transform.Position = position;
    }
}