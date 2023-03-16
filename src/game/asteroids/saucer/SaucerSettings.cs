using Worms.engine.data;
using Worms.engine.game_object;

namespace Worms.game.asteroids.saucer; 

public readonly struct SaucerSettings {
    public readonly Transform parent;
    public readonly Vector2 position;
    public readonly Transform? target;
    public readonly bool goesToTheRight;
    public readonly bool big;
    public readonly float skillRatio;

    public SaucerSettings(Transform parent, Vector2 position, Transform? target, bool goesToTheRight, bool big, float skillRatio) {
        this.parent = parent;
        this.position = position;
        this.target = target;
        this.goesToTheRight = goesToTheRight;
        this.big = big;
        this.skillRatio = skillRatio;
    }
}