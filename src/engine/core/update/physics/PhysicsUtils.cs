using Worms.engine.data;

namespace Worms.engine.core.update.physics; 

public static class PhysicsUtils {
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

    public static bool LineBoxIntersectionWithNormal(
        Vector2 bottomLeft,
        Vector2 topLeft,
        Vector2 topRight,
        Vector2 bottomRight,
        Vector2 origin,
        Vector2 direction,
        out Tuple<Vector2, Vector2> value
    ) {
        value = new Tuple<Vector2, Vector2>(Vector2.Zero(), Vector2.Zero());
        Tuple<Vector2, Vector2>? hit = CalculateBoxIntersectionPointWithNormal(
            bottomLeft,
            topLeft,
            topRight,
            bottomRight,
            origin,
            direction
        );
        if (hit == null) {
            return false;
        }

        value = hit;
        return true;
    }
    
    private static Tuple<Vector2, Vector2>? CalculateBoxIntersectionPointWithNormal(
        Vector2 bottomLeft,
        Vector2 topLeft,
        Vector2 topRight,
        Vector2 bottomRight,
        Vector2 origin,
        Vector2 direction
    ) {
        Tuple<Vector2, Vector2>? best = null;
        if (LineLineIntersection(origin, direction, bottomLeft, topLeft, out Vector2 p1)) {
            best = new Tuple<Vector2, Vector2>(p1, Vector2.Left());
        }
        if (LineLineIntersection(origin, direction, bottomLeft, bottomRight, out Vector2 p2)) {
            Tuple<Vector2, Vector2> hit = new(p2, Vector2.Down());
            best = best == null || best.Item1.SqrMagnitude > hit.Item1.SqrMagnitude ? hit : best; 
        }
        if (LineLineIntersection(origin, direction, topLeft, topRight, out Vector2 p3)) {
            Tuple<Vector2, Vector2> hit = new(p3, Vector2.Up());
            best = best == null || best.Item1.SqrMagnitude > hit.Item1.SqrMagnitude ? hit : best; 
        }
        if (LineLineIntersection(origin, direction, bottomRight, topRight, out Vector2 p4)) {
            Tuple<Vector2, Vector2> hit = new(p4, Vector2.Right());
            best = best == null || best.Item1.SqrMagnitude > hit.Item1.SqrMagnitude ? hit : best; 
        }

        return best;
    }
    
    private static bool LineLineIntersection(
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
}