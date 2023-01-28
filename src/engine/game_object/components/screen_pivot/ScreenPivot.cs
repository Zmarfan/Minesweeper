using Worms.engine.core;
using Worms.engine.core.window;
using Worms.engine.data;
using Worms.engine.game_object.scripts;
using EventHandler = Worms.engine.core.event_handler.EventHandler;

namespace Worms.engine.game_object.components.screen_pivot; 

public class ScreenPivot : Script {
    private readonly Vector2 _pivot;
    private Vector2 _oldResolution;
    
    public ScreenPivot(Vector2 pivot, bool isActive) : base(isActive) {
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

    private void SetPivot(Vector2 resolution) {
        Transform.Position = resolution / -2 + resolution * _pivot;
    }
}