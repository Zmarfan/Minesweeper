using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class RotateSizeScript : Script {
    private int _scaleDirection = 1;
    private float _speed;
    
    public RotateSizeScript(bool isActive, float speed) : base(isActive) {
        _speed = speed;
    }

    public override void Update(float deltaTime) {
        Transform.LocalRotation += deltaTime * _speed;
        Transform.LocalScale += deltaTime * _speed * 0.01f * _scaleDirection;
        if (Transform.LocalScale is > 1.5f or < 0.5f) {
            _scaleDirection *= -1;
        }
    }
}