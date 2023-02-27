using Worms.engine.data;

namespace Worms.engine.game_object.components.physics; 

public class RigidBody : ToggleComponent {
    public Vector2 velocity;
    public float angularVelocity;
    public Vector2 force;
    public float Mass {
        get => InverseMass == 0 ? 0 : 1 / InverseMass;
        set => InverseMass = value == 0 ? 0 : 1 / value;
    }
    public float gravityScale;
    public float friction;
    public float bounciness;

    public float InverseMass { get; private set; }

    public RigidBody(
        bool isActive,
        Vector2 velocity,
        float angularVelocity,
        Vector2 force,
        float mass,
        float gravityScale,
        float friction,
        float bounciness
    ) : base(isActive) {
        this.velocity = velocity;
        this.angularVelocity = angularVelocity;
        this.force = force;
        Mass = mass;
        this.gravityScale = gravityScale;
        this.friction = friction;
        this.bounciness = bounciness;
    }

    public void AddForce(Vector2 addForce) {
        force += addForce;
    }
}