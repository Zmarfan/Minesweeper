using Worms.engine.data;

namespace Worms.engine.core.update; 

public static class PhysicsUtils {
    public static bool LineIntersection(
        Vector2 origin, 
        Vector2 direction,
        Vector2 p1,
        Vector2 p2,
        out Vector2 intersection
    ) {
        intersection = Vector2.Zero();
        
        float denominator = Vector2.Cross(direction, p2 - p1);
        if (denominator == 0) {
            return false;
        }

        float u = Vector2.Cross(p2 - p1, origin - p1) / denominator;
        float t = Vector2.Cross(direction, origin - p1) / denominator;
        if (u is < 0 or > 1 || t is < 0 or > 1) {
            return false;
        }
        intersection = origin + u * direction;
        return true;
    }

    public static bool LineCircleIntersection(
        Vector2 origin,
        Vector2 direction, 
        Vector2 center,
        float radius,
        out Vector2 intersection
    ) {
        intersection = Vector2.Zero();
        
        Vector2 toCircle = origin - center;
        float a = Vector2.Dot(direction, direction);
        float b = 2 * Vector2.Dot(toCircle, direction);
        float c = Vector2.Dot(toCircle, toCircle) - radius * radius;

        float det = b * b - 4 * a * c;
        
        if (det <= 0) {
            return false;
        }
        float t = (float)(-b - Math.Sqrt(det)) / (2 * a);
        if (t is >= 0 and <= 1) {
            intersection = origin + direction * t;
            return true;
        }

        return false;
    }
}