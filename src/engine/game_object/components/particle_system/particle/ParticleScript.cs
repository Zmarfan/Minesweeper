using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.particle_system.particle; 

public class MoveParticleScript : Script {
    private readonly ClockTimer _lifeTimer;
    private Vector2 _direction;
    private readonly Vector2 _force;
    
    public MoveParticleScript(float lifeTime, Vector2 direction, Vector2 force) : base(true) {
        _lifeTimer = new ClockTimer(lifeTime);
        _direction = direction;
        _force = force;
    }

    public override void Update(float deltaTime) {
        _lifeTimer.Time += deltaTime;
        if (_lifeTimer.Expired()) {
            gameObject.Destroy();
        }

        _direction += _force * deltaTime;
        Transform.Position += _direction * deltaTime;
    }
}