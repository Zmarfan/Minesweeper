using Worms.engine.data;

namespace Worms.engine.core.update.physics; 

public record ColliderHit(Vector2 point, Vector2 normal) {
    public Vector2 point = point;
    public Vector2 normal = normal;
}