using Worms.engine.data;
using Worms.engine.game_object.components;
using Worms.engine.game_object.components.physics.colliders;

namespace Worms.engine.core.update.physics; 

public static class TriggerIntersectUtils {
    public const int CIRCLE_TO_POLYGON_POINT_COUNT = 15;
    
    public static bool DoesBoxOnBoxOverlap(BoxCollider c1, BoxCollider c2) {
        return c1.Transform.Rotation == c2.Transform.Rotation
            ? DoSameRotationBoxesOverlap(c1, c2)
            : DoConvexPolygonsOverlap(c1.WorldCorners, c2.WorldCorners);
    }

    public static bool DoesCircleOnCircleOverlap(CircleCollider c1, CircleCollider c2) {
        if (IsScaleUniform(c1) && IsScaleUniform(c2)) {
            return DoCirclesOverlap(c1.Center, c2.Center, c1.radius * c1.Transform.Scale.x, c2.radius * c2.Transform.Scale.x);
        }

        // This check is an approximation and NOT exact mathematically. Ellipses are weird and hard, no fun ):
        // Here we transform one of the ellipses to a convex polygon and then we check if the circle intersect it
        // the downside with this is that we lose precision in narrow parts 
        return DoCirclePolygonOverlap(c1, c2, c2.GetCircleAsPoints(CIRCLE_TO_POLYGON_POINT_COUNT));
    }

    public static bool DoesPixelOnColliderOverlap(PixelCollider pixel, Collider collider) {
        Tuple<Vector2, Vector2> box = CalculatePixelTextureBoundingBox(pixel, collider);

        for (int x = (int)Math.Max(box.Item1.x, 0); x < Math.Min(box.Item2.x, pixel.Width); x++) {
            for (int y = (int)Math.Max(box.Item1.y, 0); y < Math.Min(box.Item2.y, pixel.Height); y++) {
                if (!pixel.pixels[x, y].IsOpaque) {
                    continue;
                }

                Vector2 world = pixel.Transform.LocalToWorldMatrix.ConvertPoint(pixel.PixelToLocal(new Vector2Int(x, y)));
                if (collider.IsPointInside(world)) {
                    return true;
                }
            }
        }

        return false;
    }

    private static Tuple<Vector2, Vector2> CalculatePixelTextureBoundingBox(PixelCollider pixel, Collider collider) {
        return CalculateBoundingBox(
            collider.GetLocalCorners()
                .Select(c => collider.Transform.LocalToWorldMatrix.ConvertPoint(c))
                .Select(c => pixel.Transform.WorldToLocalMatrix.ConvertPoint(c))
                .Select(pixel.LocalToPixel)
                .Select(c => new Vector2(c.x, c.y))
                .ToList()
        );
    }

    public static bool DoesBoxOnCircleOverlap(CircleCollider c1, BoxCollider c2) {
        return DoCirclePolygonOverlap(c1, c2, c2.WorldCorners);
    }

    public static bool DoBoundingBoxesOverlap(Collider c1, Collider c2) {
        Tuple<Vector2, Vector2> bounding1 = CalculateBoundingBox(
            c1.GetLocalCorners().Select(c => c1.Transform.LocalToWorldMatrix.ConvertPoint(c)).ToList()
        );
        Tuple<Vector2, Vector2> bounding2 = CalculateBoundingBox(
            c2.GetLocalCorners().Select(c => c2.Transform.LocalToWorldMatrix.ConvertPoint(c)).ToList()
        );
        return DoRectanglesOverlap(bounding1.Item1, bounding1.Item2, bounding2.Item1, bounding2.Item2);
    }
    
    private static bool DoSameRotationBoxesOverlap(BoxCollider c1, BoxCollider c2) {
        Vector2 c2BottomLeft = c1.Transform.WorldToLocalMatrix.ConvertPoint(
            c2.Transform.LocalToWorldMatrix.ConvertPoint(c2.BottomLeftLocal)
        );
        Vector2 c2TopRight = c1.Transform.WorldToLocalMatrix.ConvertPoint(
            c2.Transform.LocalToWorldMatrix.ConvertPoint(c2.TopRightLocal)
        );
        return DoRectanglesOverlap(c1.BottomLeftLocal, c1.TopRightLocal, c2BottomLeft, c2TopRight);
    }

    private static bool DoRectanglesOverlap(Vector2 bottomLeft1, Vector2 topRight1, Vector2 bottomLeft2, Vector2 topRight2) {
        return bottomLeft1.x < topRight2.x
               && topRight1.x > bottomLeft2.x
               && bottomLeft1.y < topRight2.y
               && topRight1.y > bottomLeft2.y;
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

    private static bool DoCirclesOverlap(
        Vector2 c1Center,
        Vector2 c2Center,
        float c1Radius,
        float c2Radius
    ) {
        Vector2 distance = c1Center - c2Center;
        float radiusSum = c1Radius + c2Radius;
        return distance.x * distance.x + distance.y * distance.y <= radiusSum * radiusSum;
    }

    private static bool DoCirclePolygonOverlap(CircleCollider c1, Collider c2, IReadOnlyList<Vector2> c2Points) {
        if (c1.IsPointInside(c2.Center) || c2.IsPointInside(c1.Center)) {
            return true;
        }

        int fromIndex = c2Points.Count - 1;
        for (int toIndex = 0; toIndex < c2Points.Count; toIndex++) {
            Vector2 origin = c1.Transform.WorldToLocalMatrix.ConvertPoint(c2Points[fromIndex]);
            Vector2 direction = c1.Transform.WorldToLocalMatrix.ConvertPoint(c2Points[toIndex]) - origin;
            if (PhysicsUtils.LineCircleIntersection(origin, direction, c1.offset, c1.radius, out _)) {
                return true;
            }
            fromIndex = toIndex;
        }
        
        return false;
    }
    
    private static bool IsScaleUniform(Component c) {
        return Math.Abs(c.Transform.Scale.x - c.Transform.Scale.y) < 0.001f;
    }

    private static Tuple<Vector2, Vector2> CalculateBoundingBox(IReadOnlyCollection<Vector2> corners) {
        float minX = corners.MinBy(c => c.x).x;
        float minY = corners.MinBy(c => c.y).y;
        float maxX = corners.MaxBy(c => c.x).x;
        float maxY = corners.MaxBy(c => c.y).y;
        return new Tuple<Vector2, Vector2>(new Vector2(minX, minY), new Vector2(maxX, maxY));
    }
}