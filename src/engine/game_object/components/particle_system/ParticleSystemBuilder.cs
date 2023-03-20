using GameEngine.engine.data;
using GameEngine.engine.game_object.components.animation.animation;
using GameEngine.engine.game_object.components.particle_system.emission;
using GameEngine.engine.game_object.components.particle_system.particles;
using GameEngine.engine.game_object.components.particle_system.ranges;
using GameEngine.engine.game_object.components.particle_system.renderer;
using GameEngine.engine.game_object.components.particle_system.shape;
using GameEngine.engine.helper;

namespace GameEngine.engine.game_object.components.particle_system; 

public class ParticleSystemBuilder {
    private Particles _particles = ParticlesBuilder.Builder().Build();
    private Emission _emission = new(new RangeZero(10f), ListUtils.Empty<EmissionBurst>());
    private Shape _shape = new(new SphereEmission(10f, 0, Rotation.FromDegrees(359)), new VectorRange(new Vector2(5f, 5f)));
    private ParticleSystemBuilder? _subSystem;
    private Func<Animation>? _particleAnimationProvider; 
    private readonly Renderer _renderer;
    private bool _isActive = true;
    private string _name = "particleSystem";

    private ParticleSystemBuilder(Renderer renderer) {
        _renderer = renderer;
    }

    public ParticleSystem Build() {
        return new ParticleSystem(
            _particles,
            _emission.Clone(),
            _shape,
            _renderer,
            _subSystem,
            _particleAnimationProvider,
            _isActive,
            _name
        );
    }
    
    public static ParticleSystemBuilder Builder(Renderer renderer) {
        return new ParticleSystemBuilder(renderer);
    }

    public ParticleSystemBuilder SetParticles(Particles particles) {
        _particles = particles;
        return this;
    }
    
    public ParticleSystemBuilder SetEmission(Emission emission) {
        _emission = emission;
        return this;
    }
    
    public ParticleSystemBuilder SetShape(Shape shape) {
        _shape = shape;
        return this;
    }

    public ParticleSystemBuilder SetSubSystem(ParticleSystemBuilder subSystem) {
        _subSystem = subSystem;
        return this;
    }

    public ParticleSystemBuilder SetParticleAnimation(Func<Animation>? animationProvider) {
        _particleAnimationProvider = animationProvider;
        return this;
    }
    
    public ParticleSystemBuilder SetIsActive(bool isActive) {
        _isActive = isActive;
        return this;
    }
    
    public ParticleSystemBuilder SetName(string name) {
        _name = name;
        return this;
    }
}