using Worms.engine.data;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.scripts;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.asteroids; 

public class Asteroid : Script {
    private readonly Vector2 _velocity;
    private readonly float _angularVelocity;
    
    public Asteroid(Vector2 velocity, float angularVelocity) : base(true) {
        _velocity = velocity;
        _angularVelocity = angularVelocity;
    }

    public override void Update(float deltaTime) {
        Transform.Position += _velocity * deltaTime;
        Transform.Rotation += _angularVelocity * deltaTime;
    }

    public override void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.Tag == TagNames.SHOT) {
            collider.gameObject.Destroy();
        }
    }
}