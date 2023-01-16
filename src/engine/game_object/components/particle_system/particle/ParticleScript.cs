using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.particle_system.particle; 

public class ParticleScript : Script {
    private Vector2 _oldParentWorld;
    private readonly ClockTimer _lifeTimer;
    private readonly float _rotationVelocity;
    private Vector2 _direction;
    private readonly Vector2 _force;
    
    public ParticleScript(float lifeTime, float rotationVelocity, Vector2 direction, Vector2 force) : base(true) {
        _lifeTimer = new ClockTimer(lifeTime);
        _rotationVelocity = rotationVelocity;
        _direction = direction;
        _force = force;
    }

    public override void Update(float deltaTime) {
        HandleLifeTime(deltaTime);
        CalculateRotation(deltaTime);
        CalculateDirectionByForce(deltaTime);
        MoveAlongDirection(deltaTime);
    }

    private void HandleLifeTime(float deltaTime) {
        _lifeTimer.Time += deltaTime;
        if (_lifeTimer.Expired()) {
            gameObject.Destroy();
        }
    }

    private void CalculateRotation(float deltaTime) {
        Transform.Rotation += _rotationVelocity * deltaTime;
    }
    
    private void CalculateDirectionByForce(float deltaTime) {
        _direction += _force * deltaTime;
    }
    
    private void MoveAlongDirection(float deltaTime) {
        Transform.Position += _direction * deltaTime;
    }
}