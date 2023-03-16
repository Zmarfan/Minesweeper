using Worms.engine.core.input;
using Worms.engine.data;
using Worms.engine.game_object.components.animation.controller;
using Worms.engine.game_object.components.audio_source;
using Worms.engine.game_object.components.particle_system.ranges;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.scripts;
using Worms.game.asteroids.asteroids;
using Worms.game.asteroids.names;
using Worms.game.asteroids.saucer;

namespace Worms.game.asteroids.player; 

public class PlayerMovement : Script {
    public delegate void PlayerDieDelegate();
    public static event PlayerDieDelegate? PlayerDieEvent;
    
    public const string THRUST_ANIMATION_TRIGGER = "thrust";
    public const string THRUST_AUDIO_SOURCE = "thrust";
    public const string FIRE_AUDIO_SOURCE = "fire";
    public static readonly Vector2[] COLLIDER_VERTICES = { new(-15, 12), new(25, 0), new(-15, -12) };
    
    private const float ROTATE_SPEED = 100;
    private const float THRUST_SPEED = 10;
    private const float MAX_THRUST_SPEED = 125;
    private const float DE_ACCELERATION_FRACTION = 0.975f;
    
    private AnimationController _animationController = null!;
    private AudioSource _thrustAudioSource = null!;
    private AudioSource _fireAudioSource = null!;
    
    private Vector2 _velocity = Vector2.Zero();

    private float _rotateAmount;
    private float _thrust;

    private bool _dead = false;

    private Vector2 ShotSpawnPosition => Transform.Position + Transform.Right * 30;
    
    public PlayerMovement() : base(true) {
    }

    public override void Awake() {
        _animationController = GetComponent<AnimationController>();
        List<AudioSource> audioSources = GetComponents<AudioSource>();
        _thrustAudioSource = audioSources.First(source => source.sourceName == THRUST_AUDIO_SOURCE);
        _fireAudioSource = audioSources.First(source => source.sourceName == FIRE_AUDIO_SOURCE);

        if (Input.GetButton(InputNames.THRUST)) {
            TurnThrusterEffectsOn();
        }
    }

    public override void Update(float deltaTime) {
        _rotateAmount += Input.GetAxis(InputNames.ROTATE).x;
        
        HandleThrust();

        if (Input.GetButtonDown(InputNames.FIRE)) {
            _fireAudioSource.Restart();
            float initialSpeed = Vector2.Dot(_velocity, Transform.Right);
            ShotFactory.Create(Transform.GetRoot(), ShotSpawnPosition, Transform.Right, initialSpeed * THRUST_SPEED, true);
        }
    }

    public override void FixedUpdate(float deltaTime) {
        Transform.Rotation += _rotateAmount * ROTATE_SPEED * deltaTime;
        _velocity *= DE_ACCELERATION_FRACTION;
        if (_thrust != 0) {
            _velocity += _thrust * Transform.Right;
        }
        if (_velocity.SqrMagnitude >= MAX_THRUST_SPEED * MAX_THRUST_SPEED) {
            _velocity = _velocity.Normalized * MAX_THRUST_SPEED;
        }

        Transform.LocalPosition += _velocity * THRUST_SPEED * deltaTime;
        
        _rotateAmount = 0;
        _thrust = 0;
    }

    public override void OnTriggerEnter(Collider collider) {
        if (_dead) {
            return;
        }

        _dead = true;
        
        switch (collider.gameObject.Tag) {
            case TagNames.SHOT:
                collider.gameObject.Destroy();
                break;
            case TagNames.ENEMY:
                collider.GetComponentInChildren<SaucerShooter>().Die();
                break;
        }
        Die();
    }

    private void HandleThrust() {
        _thrust += Input.GetAxis(InputNames.THRUST).x;
        
        if (Input.GetButtonDown(InputNames.THRUST)) {
            TurnThrusterEffectsOn();
        }

        if (Input.GetButtonUp(InputNames.THRUST)) {
            _animationController.Stop();
            _thrustAudioSource.loop = false;
        }
    }

    private void TurnThrusterEffectsOn() {
        _animationController.SetTrigger(THRUST_ANIMATION_TRIGGER);
        _thrustAudioSource.loop = true;
        _thrustAudioSource.Play();
    }

    public void Die() {
        ExplosionFactory.CreateExplosion(Transform.GetRoot(), Transform.Position, new RangeZero(10, 20), SoundNames.BANG_MEDIUM);
        PlayerDieEvent?.Invoke();
        gameObject.Destroy();
    }
}