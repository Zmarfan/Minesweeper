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

        float u = Vector2.Cross(p2 - p1, origin - p1) / denominator;
        float t = Vector2.Cross(direction, origin - p1) / denominator;
        if (u < 0 || t is < 0 or > 1) {
            intersection = null;
            return false;
        }
        intersection = origin + u * direction;
        return true;
    }
}