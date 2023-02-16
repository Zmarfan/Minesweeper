using Worms.engine.data;
using Worms.engine.game_object.components.animation.animation;
using Worms.engine.game_object.components.animation.controller;
using Worms.engine.game_object.components.particle_system.emission;
using Worms.engine.game_object.components.particle_system.particles;
using Worms.engine.game_object.components.particle_system.ranges;
using Worms.engine.game_object.components.particle_system.renderer;
using Worms.engine.game_object.components.particle_system.shape;
using Worms.engine.game_object.scripts;
using Worms.engine.helper;

namespace Worms.engine.game_object.components.particle_system; 

public class ParticleSystemBuilder {
    private Particles _particles = ParticlesBuilder.Builder().Build();
    private Emission _emission = new(new RangeZero(10f), ListUtils.Empty<EmissionBurst>());
    private Shape _shape = new(new SphereEmission(10f, 0, Rotation.FromDegrees(359)), new VectorRange(new Vector2(5f, 5f)), 0f);
    private ParticleSystemBuilder? _subSystem = null;
    private Func<Animation>? _particleAnimationProvider = null; 
    private readonly Renderer _renderer;
    private bool _isActive = true;

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
            _isActive
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
}