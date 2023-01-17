using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.particle_system.particle; 

public class ParticleScript : Script {
    private readonly bool _localSpace;
    private Vector2 _parentOldPosition;
    private Rotation _parentOldRotation;
    private readonly ClockTimer _lifeTimer;
    private readonly float _rotationVelocity;
    private Vector2 _direction;
    private readonly Vector2 _force;
    
    public ParticleScript(bool localSpace, float lifeTime, float rotationVelocity, Vector2 direction, Vector2 force) : base(true) {
        _localSpace = localSpace;
        _lifeTimer = new ClockTimer(lifeTime);
        _rotationVelocity = rotationVelocity;
        _direction = direction;
        _force = force;
    }

    public override void Awake() {
        _parentOldPosition = Transform.Parent!.WorldPosition;
        _parentOldRotation = Transform.Parent!.WorldRotation;
    }

    public override void Update(float deltaTime) {
        if (!_localSpace) {
            TransformPointToWorldSpace();
        }

        HandleLifeTime(deltaTime);
        CalculateRotation(deltaTime);
        CalculateDirectionByForce(deltaTime);
        MoveAlongDirection(deltaTime);
    }

    private void TransformPointToWorldSpace() {
        Transform.WorldPosition += _parentOldPosition - Transform.Parent!.WorldPosition;
        Transform.WorldPosition = RotatePoint(_parentOldPosition, Transform.Parent!.WorldRotation.Radians - _parentOldRotation.Radians, Transform.WorldPosition);
        Transform.WorldRotation -= Transform.Parent!.WorldRotation - _parentOldRotation;
        
        _parentOldPosition = Transform.Parent!.WorldPosition;
        _parentOldRotation = Transform.Parent!.WorldRotation;
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
        if (_localSpace) {
            Transform.Position += _direction * deltaTime;
        }
        else {
            Transform.WorldPosition += _direction * deltaTime;
        }
    }
    
    Vector2 RotatePoint(Vector2 center, float radians, Vector2 point)
    {
        float s = (float)Math.Sin(radians);
        float c = (float)Math.Cos(radians);

        point -= center;

        Vector2 rotated = new(point.x * c - point.y * s, point.x * s + point.y * c);

        return rotated + center;
    }
}