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

public class PlayerBase : Script {
    public delegate void PlayerDieDelegate();
    public static event PlayerDieDelegate? PlayerDieEvent;
    
    public static readonly Vector2[] COLLIDER_VERTICES = { new(-15, 12), new(25, 0), new(-15, -12) };
    
    private bool _dead = false;
    
    public PlayerBase() : base(true) {
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
    
    public void Die() {
        ExplosionFactory.CreateShipExplosion(Transform.GetRoot(), Transform.Position);
        ExplosionFactory.CreateExplosion(Transform.GetRoot(), Transform.Position, new RangeZero(5, 15));
        PlayerDieEvent?.Invoke();
        gameObject.Destroy();
    }
}