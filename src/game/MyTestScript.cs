using Worms.engine.core.input;
using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class MyTestScript : Script {
    private readonly float _speed;
    
    public MyTestScript(float speed) : base(true) {
        _speed = speed;
    }

    public override void Update(float deltaTime) {
        Transform.WorldPosition += Input.GetAxis("horizontal") * _speed * 1000 * deltaTime;
        Transform.WorldPosition += Input.GetAxis("vertical") * _speed * 1000 * deltaTime;
        Transform.WorldRotation += Input.GetButton("action") ? _speed * 50 * deltaTime : 0;
    }
}