using Worms.engine.data;
using Worms.engine.game_object.components.particle_system.ranges;
using Range = Worms.engine.game_object.components.particle_system.ranges.Range;

namespace Worms.engine.game_object.components.particle_system.particles; 

public class ParticlesBuilder {
    private bool _localSpace = true;
    private float _duration = 5f;
    private bool _loop = true;
    private RangeZero _startDelay = new(0f);
    private RangeZero _startLifeTime = new(5f);
    private RangeZero _startSize = new(1f);
    private RangeZero _startRotation = new(Rotation.Identity().Degree);
    private float _flipRotation;
    private bool _playOnAwake = true;
    private int _maxParticles = 1000;
    private int _seed = Guid.NewGuid().GetHashCode();
    private StopAction _stopAction = StopAction.NONE;
    private Range _rotationVelocity = new(0);
    private VectorRange _forceOverLifeTime = new(Vector2.Zero());

    public static ParticlesBuilder Builder() {
        return new ParticlesBuilder();
    }

    public Particles Build() {
        return new Particles(
            _localSpace,
            _duration,
            _loop,
            _startDelay,
            _startLifeTime,
            _startSize,
            _startRotation,
            _flipRotation,
            _playOnAwake,
            _maxParticles,
            _seed,
            _stopAction,
            _rotationVelocity,
            _forceOverLifeTime
        );
    }

    public ParticlesBuilder SetLocalSpace() {
        _localSpace = true;
        return this;
    }
    
    public ParticlesBuilder SetWorldSpace() {
        _localSpace = false;
        return this;
    }
    
    public ParticlesBuilder SetDuration(float duration) {
        _duration = duration;
        return this;
    }
    
    public ParticlesBuilder SetLoop(bool loop) {
        _loop = loop;
        return this;
    }
    
    public ParticlesBuilder SetStartDelay(RangeZero range) {
        _startDelay = range;
        return this;
    }
    
    public ParticlesBuilder SetStartLifeTime(RangeZero range) {
        _startLifeTime = range;
        return this;
    }

    public ParticlesBuilder SetStartSize(RangeZero range) {
        _startSize = range;
        return this;
    }
            
    public ParticlesBuilder SetStartRotation(RangeZero range) {
        _startRotation = range;
        return this;
    }
    
    public ParticlesBuilder SetFlipRotation(float ratio) {
        _flipRotation = ratio;
        return this;
    }
    
    public ParticlesBuilder SetPlayOnAwake(bool playOnAwake) {
        _playOnAwake = playOnAwake;
        return this;
    }
    
    public ParticlesBuilder SetMaxParticles(int maxParticles) {
        _maxParticles = maxParticles;
        return this;
    }
    
    public ParticlesBuilder SetSeed(int seed) {
        _seed = seed;
        return this;
    }
    
    public ParticlesBuilder SetStopAction(StopAction stopAction) {
        _stopAction = stopAction;
        return this;
    }

    public ParticlesBuilder SetRotationVelocity(Range range) {
        _rotationVelocity = range;
        return this;
    }
    
    public ParticlesBuilder SetForceOverLifeTime(VectorRange force) {
        _forceOverLifeTime = force;
        return this;
    }
}