using Worms.engine.core.input;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class MyTestScript : Script {
    private readonly float _speed;
    
    public MyTestScript(float speed) : base(true) {
        _speed = speed;
    }

    public override void Update(float deltaTime) {
        Transform.LocalPosition += Input.GetAxis("horizontal") * _speed * 1000 * deltaTime;
        Transform.LocalPosition += Input.GetAxis("vertical") * _speed * 1000 * deltaTime;
        Transform.LocalRotation += Input.GetButton("action") ? _speed * 50 * deltaTime : 0;
    }
}