using Worms.engine.core.renderer;
using Worms.engine.game_object.components.particle_system.emission;
using Worms.engine.game_object.components.particle_system.particles;
using Worms.engine.game_object.components.particle_system.shape;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.particle_system; 

public class ParticleSystem : Script {
    public ParticleSystem(
        Particles particles,
        Emission emission,
        Shape shape,
        Renderer renderer,
        bool isActive
    ) : base(isActive) {
    }
}