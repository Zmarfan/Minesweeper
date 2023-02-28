using Worms.engine.data;

namespace Worms.engine.core.update.physics.updating; 

public readonly struct CollisionData {
    public readonly Vector2 normal;
    public readonly float penetration;

    public CollisionData(Vector2 normal, float penetration) {
        this.normal = normal;
        this.penetration = penetration;
    }
}