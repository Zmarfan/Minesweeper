using GameEngine.engine.data;
using GameEngine.engine.game_object.scripts;

namespace GameEngine.engine.game_object.components.particle_system.particle; 

internal class ParticleScript : Script {
    private readonly bool _localSpace;
    private Vector2 _parentOldPosition;
    private Rotation _parentOldRotation;
    private readonly ClockTimer _lifeTimer;
    private readonly float _rotationVelocity;
    private Vector2 _direction;
    private readonly Vector2 _force;
    
    public ParticleScript(bool localSpace, float lifeTime, float rotationVelocity, Vector2 direction, Vector2 force) : base(true, "particle") {
        _localSpace = localSpace;
        _lifeTimer = new ClockTimer(lifeTime);
        _rotationVelocity = rotationVelocity;
        _direction = direction;
        _force = force;
    }

    public override void Awake() {
        _parentOldPosition = Transform.Parent!.Position;
        _parentOldRotation = Transform.Parent!.Rotation;
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
        Transform.Position += _parentOldPosition - Transform.Parent!.Position;
        Transform.Position = Vector2.RotatePoint(_parentOldPosition, _parentOldRotation - Transform.Parent!.Rotation, Transform.Position);
        Transform.Rotation -= Transform.Parent!.Rotation - _parentOldRotation;
        
        _parentOldPosition = Transform.Parent!.Position;
        _parentOldRotation = Transform.Parent!.Rotation;
    }

    private void HandleLifeTime(float deltaTime) {
        _lifeTimer.Time += deltaTime;
        if (_lifeTimer.Expired()) {
            gameObject.Destroy();
        }
    }

    private void CalculateRotation(float deltaTime) {
        Transform.LocalRotation += _rotationVelocity * deltaTime;
    }
    
    private void CalculateDirectionByForce(float deltaTime) {
        _direction += _force * deltaTime;
    }
    
    private void MoveAlongDirection(float deltaTime) {
        if (_localSpace) {
            Transform.LocalPosition += _direction * deltaTime;
        }
        else {
            Transform.Position += _direction * deltaTime;
        }
    }
}