using Worms.engine.core.gizmos;
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
        Transform.Position += Input.GetAxis("horizontal") * _speed * 1000 * deltaTime;
        Transform.Position += Input.GetAxis("vertical") * _speed * 1000 * deltaTime;
        Transform.Rotation += Input.GetButton("action") ? _speed * 50 * deltaTime : 0;
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawLine(Transform.Position, Transform.Position + new Vector2(-1000f, 1000f), new Color(1f, 0.5f, 0.75f));
    }
}