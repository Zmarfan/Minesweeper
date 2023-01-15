using Worms.engine.data;
using Worms.engine.game_object.components.particle_system.emission;
using Worms.engine.game_object.components.particle_system.particle;
using Worms.engine.game_object.components.particle_system.particles;
using Worms.engine.game_object.components.particle_system.renderer;
using Worms.engine.game_object.components.particle_system.shape;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.particle_system; 

public class ParticleSystem : Script {
    private readonly Particles _particles;
    private readonly Emission _emission;
    private readonly Shape _shape;
    private readonly Renderer _renderer;
    
    public ParticleSystem(
        Particles particles,
        Emission emission,
        Shape shape,
        Renderer renderer,
        bool isActive
    ) : base(isActive) {
        _particles = particles;
        _emission = emission;
        _shape = shape;
        _renderer = renderer;
    }

    public override void Update(float deltaTime) {
    }
}