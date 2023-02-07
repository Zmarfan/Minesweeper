using Worms.engine.data;
using Worms.engine.game_object.components.physics.colliders;

namespace Worms.engine.core.update.physics; 

public static class TriggerIntersectUtils {
    public static bool DoesBoxOnBoxOverlap(BoxCollider c1, BoxCollider c2) {
        return c1.Transform.Rotation == c2.Transform.Rotation
            ? DoRectanglesOverlap(c1, c2)
            : DoConvexPolygonsOverlap(c1.WorldCorners, c2.WorldCorners);
    }

    public static bool DoesCircleOnCircleOverlap(Collider c1, Collider c2) {
        return false;
    }

    public static bool DoesPixelOnPixelOverlap(Collider c1, Collider c2) {
        return false;
    }

    public static bool DoesBoxOnCircleOverlap(Collider c1, Collider c2) {
        return false;
    }

    public static bool DoesBoxOnPixelOverlap(Collider c1, Collider c2) {
        return false;
    }

    public static bool DoesCircleOnPixelOverlap(Collider c1, Collider c2) {
        return false;
    }
    
    private static bool DoRectanglesOverlap(BoxCollider c1, BoxCollider c2) {
        Vector2 c2BottomLeft = c1.Transform.WorldToLocalMatrix.ConvertPoint(
            c2.Transform.LocalToWorldMatrix.ConvertPoint(c2.BottomLeftLocal)
        );
        Vector2 c2TopRight = c1.Transform.WorldToLocalMatrix.ConvertPoint(
            c2.Transform.LocalToWorldMatrix.ConvertPoint(c2.TopRightLocal)
        );
        return c1.BottomLeftLocal.x < c2TopRight.x
               && c1.TopRightLocal.x > c2BottomLeft.x
               && c1.BottomLeftLocal.y < c2TopRight.y
               && c1.TopRightLocal.y > c2BottomLeft.y;
    }
    
    private static bool DoConvexPolygonsOverlap(List<Vector2> c1Points, List<Vector2> c2Points) {
        foreach (List<Vector2> points in new[] { c1Points, c2Points })
        {
            for (int i1 = 0; i1 < points.Count; i1++)
            {
                int i2 = (i1 + 1) % points.Count;
                Vector2 normal = new(points[i2].y - points[i1].y, points[i1].x - points[i2].x);

                FindMinMaxBoxPointsAlongNormal(c1Points, normal, out float minA, out float maxA);
                FindMinMaxBoxPointsAlongNormal(c2Points, normal, out float minB, out float maxB);

                if (maxA < minB || maxB < minA) {
                    return false;
                }
            }
        }
        return true;
    }
    
    private static void FindMinMaxBoxPointsAlongNormal(IEnumerable<Vector2> corners, Vector2 normal, out float min, out float max) {
        min = float.MaxValue;
        max = float.MinValue;
        foreach (float projected in corners.Select(p => Vector2.Dot(normal, p))) {
            min = projected < min ? projected : min;
            max = projected > max ? projected : max;
        }
    }
}