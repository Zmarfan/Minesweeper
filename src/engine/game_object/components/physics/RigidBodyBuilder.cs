using Worms.engine.data;

namespace Worms.engine.game_object.components.physics; 

public class RigidBodyBuilder {
    private bool _isActive = true;
    private Vector2 _velocity = Vector2.Zero();
    private float _angularVelocity = 0;
    private Vector2 _force = Vector2.Zero();
    private float _mass = 1;
    private float _gravityScale = 1;
    private float _friction = 0.4f;
    private float _bounciness = 0;

    public static RigidBodyBuilder Builder() {
        return new RigidBodyBuilder();
    }

    public RigidBody Build() {
        return new RigidBody(_isActive, _velocity, _angularVelocity, _force, _mass, _gravityScale, _friction, _bounciness);
    }

    public RigidBodyBuilder SetIsActive(bool isActive) {
        _isActive = isActive;
        return this;
    }
    
    public RigidBodyBuilder SetVelocity(Vector2 velocity) {
        _velocity = velocity;
        return this;
    }
    
    public RigidBodyBuilder SetAngularVelocity(float angularVelocity) {
        _angularVelocity = angularVelocity;
        return this;
    }
    
    public RigidBodyBuilder SetForce(Vector2 force) {
        _force = force;
        return this;
    }
    
    public RigidBodyBuilder SetMass(float mass) {
        _mass = mass;
        return this;
    }
    
    public RigidBodyBuilder SetGravityScale(float gravityScale) {
        _gravityScale = gravityScale;
        return this;
    }
    
    public RigidBodyBuilder SetFriction(float friction) {
        _friction = friction;
        return this;
    }
    
    public RigidBodyBuilder SetBounciness(float bounciness) {
        _bounciness = bounciness;
        return this;
    }
}