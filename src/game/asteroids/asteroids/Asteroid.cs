using Worms.engine.data;
using Worms.engine.game_object.components.particle_system.ranges;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.scripts;
using Worms.game.asteroids.names;
using Worms.game.asteroids.saucer;

namespace Worms.game.asteroids.asteroids; 

public class Asteroid : Script {
    private readonly Vector2 _velocity;
    private readonly float _angularVelocity;
    private readonly AsteroidDetails _details;
    
    public Asteroid(Vector2 velocity, float angularVelocity, AsteroidDetails details) : base(true) {
        _velocity = velocity;
        _angularVelocity = angularVelocity;
        _details = details;
    }

    public override void Update(float deltaTime) {
        Transform.Position += _velocity * deltaTime;
        Transform.Rotation += _angularVelocity * deltaTime;
    }

    public override void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.Tag is not (TagNames.SHOT or TagNames.ENEMY)) {
            return;
        }
        if (collider.gameObject.Tag == TagNames.ENEMY) {
            collider.GetComponentInChildren<SaucerShooter>().Die();
            ExplosionFactory.CreateExplosion(Transform.GetRoot(), Transform.Position, _details.particleCount);
        }
        else {
            collider.gameObject.Destroy();
            ExplosionFactory.CreateExplosion(Transform.GetRoot(), Transform.Position, _details.particleCount, _details.explosionAudioId);
        }
            
        if (_details.type != AsteroidType.SMALL) {
            SpawnNewAsteroids();
        }
        gameObject.Destroy();
    }

    private void SpawnNewAsteroids() {
        AsteroidType newType = _details.type == AsteroidType.BIG ? AsteroidType.MEDIUM : AsteroidType.SMALL;
        AsteroidFactory.Create(Transform.GetRoot(), newType, Transform.Position);
        AsteroidFactory.Create(Transform.GetRoot(), newType, Transform.Position);
    }
}