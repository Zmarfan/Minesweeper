using Worms.engine.core.game_object_handler;
using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.engine.core.update.physics.updating; 

public static class PhysicsUtils {
    public static void RunScriptsFunction(TrackObject obj, Action<Script> action) {
        foreach (Script script in obj.Scripts) {
            if (script.IsActive) {
                action.Invoke(script);
            }
        }
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

    public static bool LineBoxIntersectionWithNormal(
        Vector2[] corners,
        Vector2 origin,
        Vector2 direction,
        out Tuple<Vector2, Vector2> pointAndNormal
    ) {
        Tuple<Vector2, Vector2>? best = null;
        int fromIndex = corners.Length - 1;
        for (int i = 0; i < corners.Length; i++) {
            if (LineLineIntersection(origin, direction, corners[fromIndex], corners[i], out Vector2 p)) {
                Vector2 edgeDirection = corners[i] - corners[fromIndex];
                Vector2 normal = new(-edgeDirection.y, edgeDirection.x);
                Tuple<Vector2, Vector2> hit = new(p, normal);
                best = best == null || (origin - best.Item1).SqrMagnitude > (origin - hit.Item1).SqrMagnitude ? hit : best; 
            }
            
            fromIndex = i;
        }

        if (best == null) {
            pointAndNormal = new Tuple<Vector2, Vector2>(Vector2.Zero(), Vector2.Zero());
            return false;
        }

        pointAndNormal = best;
        return true;
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