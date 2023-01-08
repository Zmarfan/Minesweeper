using Worms.engine.data;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class MyTestScript : Script {
    private readonly ClockTimer _timer;
    private int _scaleDirection = 1;
    private TextureRenderer _textureRenderer = null!;
    
    public MyTestScript(float speed) : base(true) {
        _timer = new ClockTimer(speed);
    }

    public override void Awake() {
        _textureRenderer = GetComponent<TextureRenderer>();
    }

    public override void Update(float deltaTime) {
        _timer.Time += deltaTime;
        if (_timer.Expired()) {
            _textureRenderer.IsActive = !_textureRenderer.IsActive;
            _timer.Reset();
        }
        
        Transform.LocalRotation += deltaTime * _timer.Duration * 5f;
        Transform.LocalScale += deltaTime * _timer.Duration * 0.05f * _scaleDirection;
        if (Transform.LocalScale is > 1.5f or < 0.5f) {
            _scaleDirection *= -1;
        }
    }
}