using Worms.engine.data;
using Worms.engine.game_object.components.animation.animation;
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
    private readonly ParticleSystemBuilder? _subSystem;
    private readonly Func<Animation>? _particleAnimationProvider;
    private readonly Renderer _renderer;
    private Transform _particleHolder = null!;

    private bool _playing;
    private Random _random;
    private readonly ClockTimer _startDelayTimer;
    private readonly ClockTimer _durationTimer;
    private ClockTimer _rateOverTimeTimer;
    
    public ParticleSystem(
        Particles particles,
        Emission emission,
        Shape shape,
        Renderer renderer,
        ParticleSystemBuilder? subSystem,
        Func<Animation>? particleAnimationProvider,
        bool isActive
    ) : base(isActive) {
        _particles = particles;
        _emission = emission;
        _shape = shape;
        _subSystem = subSystem;
        _particleAnimationProvider = particleAnimationProvider;
        _renderer = renderer;

        _playing = particles.playOnAwake;
        _random = new Random(particles.seed);
        _startDelayTimer = new ClockTimer(particles.startDelay.GetRandom(_random));
        _durationTimer = new ClockTimer(particles.duration);
        _rateOverTimeTimer = CreateRateOverTimeTimer();
    }

    public override void Awake() {
        _particleHolder = Transform.Instantiate(GameObjectBuilder.Builder("particleHolder")).Transform;
    }

    public void Play() {
        _playing = true;
    }

    public void Pause() {
        _playing = false;
    }

    public void Stop() {
        _playing = false;
        _emission.Reset();
        _startDelayTimer.Reset();
        _durationTimer.Reset();
        _rateOverTimeTimer.Reset();
        _random = new Random(_particles.seed);
    }

    public override void Update(float deltaTime) {
        if (!_playing) {
            return;
        }

        _startDelayTimer.Time += deltaTime;
        if (!_startDelayTimer.Expired()) {
            return;
        }
        
        _durationTimer.Time += deltaTime;
        _rateOverTimeTimer.Time += deltaTime;
        if (_durationTimer.Expired()) {
            HandleLoopOver();
        }

        if (!_durationTimer.Expired()) {
            ExecuteFrame(deltaTime);
        }
    }

    private void ExecuteFrame(float deltaTime) {
        if (_rateOverTimeTimer.Expired()) {
            for (int i = 0; i < (int)_rateOverTimeTimer.Ratio(); i++) {
                SpawnParticle();
            }
            _rateOverTimeTimer = CreateRateOverTimeTimer();
        }

        int burstAmount = _emission.CalculateBurstAmount(deltaTime, _random);
        for (int i = 0; i < burstAmount; i++) {
            SpawnParticle();
        }
    }

    private void SpawnParticle() {
        if (_particleHolder.children.Count >= _particles.maxParticles) {
            return;
        }
        
        Tuple<Vector2, Vector2> positionAndDirection = _shape.GetSpawnPositionAndDirection(_random);
        float startSize = _particles.startSize.GetRandom(_random);
        Vector2 localForce = _particles.forceOverLifeTime.GetRandom(_random);
        
        _particleHolder.Instantiate(ParticleFactory.ParticleBuilder(
            _particles.localSpace,
            positionAndDirection.Item1,
            new Vector2(startSize, startSize),
            _particles.CalculateInitialRotation(_random),
            _particles.startLifeTime.GetRandom(_random),
            _particles.localSpace ? positionAndDirection.Item2 : Transform.LocalToWorldMatrix.ConvertVector(positionAndDirection.Item2),
            _particles.rotationVelocity.GetRandom(_random),
            _particles.localSpace ? localForce : Transform.LocalToWorldMatrix.ConvertVector(localForce),
            _subSystem,
            _particleAnimationProvider,
            _renderer
        ));
    }

    private ClockTimer CreateRateOverTimeTimer() {
        return new ClockTimer(1 / _emission.rateOverTime.GetRandom(_random));
    }
    
    private void HandleLoopOver() {
        if (_particles.loop) {
            _durationTimer.Reset();
            _emission.Reset();
            return;
        }

        switch (_particles.stopAction) {
            case StopAction.NONE:
                break;
            case StopAction.DISABLE:
                _particleHolder.gameObject.IsActive = false;
                gameObject.IsActive = false;
                break;
            case StopAction.DESTROY:
                _particleHolder.gameObject.Destroy();
                gameObject.Destroy();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}