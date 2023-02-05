using Worms.engine.data;

namespace Worms.engine.core.update; 

public static class PhysicsUtils {
    public static bool LineIntersection(
        Vector2 origin, 
        Vector2 direction,
        Vector2 p1,
        Vector2 p2,
        out Vector2? intersection
    ) {
        float denominator = Vector2.Cross(direction, p2 - p1);
        if (denominator == 0) {
            intersection = null;
            return false;
        }

        float t = Vector2.Cross(direction, origin - p1) / denominator;
        if (t is < 0 or > 1) {
            intersection = null;
            return false;
        }
        intersection = p1 + t * (p2 - p1);
        return true;
    }
}