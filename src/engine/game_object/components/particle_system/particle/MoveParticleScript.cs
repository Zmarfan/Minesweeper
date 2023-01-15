using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.particle_system.particle; 

public class MoveParticleScript : Script {
    private readonly Vector2 _direction;
    
    public MoveParticleScript(Vector2 direction) : base(true) {
        _direction = direction;
    }

    public override void Update(float deltaTime) {
        Transform.Position += _direction * deltaTime;
    }
}