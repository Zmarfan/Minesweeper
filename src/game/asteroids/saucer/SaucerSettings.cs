using Worms.engine.data;
using Worms.engine.game_object;

namespace Worms.game.asteroids.saucer; 

public readonly struct SaucerSettings {
    public readonly Transform parent;
    public readonly Vector2 position;
    public readonly Func<Transform>? targetSupplier;
    public readonly bool goesToTheRight;
    public readonly float skillRatio;

    public SaucerSettings(Transform parent, Vector2 position, Func<Transform>? targetSupplier, bool goesToTheRight, float skillRatio) {
        this.parent = parent;
        this.position = position;
        this.targetSupplier = targetSupplier;
        this.goesToTheRight = goesToTheRight;
        this.skillRatio = skillRatio;
    }
}