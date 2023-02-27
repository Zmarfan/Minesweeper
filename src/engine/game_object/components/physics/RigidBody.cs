using Worms.engine.data;

namespace Worms.engine.game_object.components.physics; 

public class RigidBody : ToggleComponent {
    public Vector2 velocity;
    public Vector2 angularVelocity;
    public Vector2 force;
    public float mass;
    public float gravityScale;

    public float InverseMass => 1 / mass;
    
    public RigidBody() : base(true) {
    }

    public void AddForce(Vector2 addForce) {
        force += addForce;
    }
}