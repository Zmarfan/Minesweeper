using Worms.engine.data;
using Worms.engine.game_object.components.particle_system.ranges;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.scripts;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.asteroids; 

public class Asteroid : Script {
    private readonly Vector2 _velocity;
    private readonly float _angularVelocity;
    private readonly AsteroidType _type;
    
    public Asteroid(Vector2 velocity, float angularVelocity, AsteroidType type) : base(true) {
        _velocity = velocity;
        _angularVelocity = angularVelocity;
        _type = type;
    }

    public override void Update(float deltaTime) {
        Transform.Position += _velocity * deltaTime;
        Transform.Rotation += _angularVelocity * deltaTime;
    }

    public override void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.Tag == TagNames.SHOT) {
            collider.gameObject.Destroy();
            ExplosionFactory.CreateExplosion(Transform.GetRoot(), Transform.Position, GetParticleCount());
            if (_type != AsteroidType.SMALL) {
                SpawnNewAsteroids();
            }
            gameObject.Destroy();
        }
    }

    private RangeZero GetParticleCount() {
        return _type switch {
            AsteroidType.BIG => new RangeZero(10, 20),
            AsteroidType.MEDIUM => new RangeZero(7, 15),
            AsteroidType.SMALL => new RangeZero(5, 12),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private void SpawnNewAsteroids() {
        AsteroidType newType = _type == AsteroidType.BIG ? AsteroidType.MEDIUM : AsteroidType.SMALL;
        AsteroidFactory.Create(Transform.GetRoot(), newType, Transform.Position);
        AsteroidFactory.Create(Transform.GetRoot(), newType, Transform.Position);
    }
}