using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.particle_system.particle; 

public class KillParticleScript : Script {
    private readonly ClockTimer _timer;
    
    public KillParticleScript(float lifeTime) : base(true) {
        _timer = new ClockTimer(lifeTime);
    }

    public override void Update(float deltaTime) {
        _timer.Time += deltaTime;
        if (_timer.Expired()) {
            gameObject.Destroy();
        }
    }
}