using Worms.engine.data;
using Worms.engine.game_object.components;
using Worms.engine.game_object.components.physics.colliders;

namespace Worms.engine.core.update.physics.updating; 

public static class IntersectUtils {
    public static bool DoTriggersIntersect(Collider c1, Collider c2) {
        if (!DoBoundingBoxesIntersect(c1, c2)) {
            return false;
        }
        
        return c1 switch {
            BoxCollider box1 when c2 is BoxCollider box2 => DoesBoxOnBoxIntersect(box1, box2),
            PolygonCollider p1 when c2 is PolygonCollider p2 => DoPolygonCollidersIntersect(p1, p2),
            CircleCollider circle1 when c2 is CircleCollider circle2 => DoesCircleOnCircleIntersect(circle1, circle2),
            PixelCollider p1 => DoesPixelOnColliderIntersect(p1, c2),
            BoxCollider box when c2 is CircleCollider circle => DoesBoxOnCircleIntersect(circle, box),
            CircleCollider circle when c2 is BoxCollider box => DoesBoxOnCircleIntersect(circle, box),
            PolygonCollider polygon when c2 is BoxCollider box => DoPolygonAndConvexPolygonIntersect(polygon, box.WorldCorners),
            BoxCollider box when c2 is PolygonCollider polygon => DoPolygonAndConvexPolygonIntersect(polygon, box.WorldCorners),
            PolygonCollider polygon when c2 is CircleCollider circle => DoPolygonAndCircleIntersect(polygon, circle),
            CircleCollider circle when c2 is PolygonCollider polygon => DoPolygonAndCircleIntersect(polygon, circle),
            not null when c2 is PixelCollider p2 => DoesPixelOnColliderIntersect(p2, c1),
            _ => throw new Exception($"The collider types: {c1} and {c2} are not supported in the physics trigger system!")
        };
    }

    private static bool DoBoundingBoxesIntersect(Collider c1, Collider c2) {
        Tuple<Vector2, Vector2> bounding1 = CalculateBoundingBox(
            c1.GetLocalCorners().Select(c => c1.Transform.LocalToWorldMatrix.ConvertPoint(c)).ToList()
        );
        Tuple<Vector2, Vector2> bounding2 = CalculateBoundingBox(
            c2.GetLocalCorners().Select(c => c2.Transform.LocalToWorldMatrix.ConvertPoint(c)).ToList()
        );
        return DoRectanglesIntersect(bounding1.Item1, bounding1.Item2, bounding2.Item1, bounding2.Item2);
    }
    
    private static bool DoesBoxOnBoxIntersect(BoxCollider c1, BoxCollider c2) {
        return c1.Transform.Rotation == c2.Transform.Rotation
            ? DoSameRotationBoxesIntersect(c1, c2)
            : DoConvexPolygonsIntersect(c1.WorldCorners, c2.WorldCorners);
    }

    private static bool DoesCircleOnCircleIntersect(CircleCollider c1, CircleCollider c2) {
        if (IsScaleUniform(c1) && IsScaleUniform(c2)) {
            return DoCirclesIntersect(c1.Center, c2.Center, c1.radius * c1.Transform.Scale.x, c2.radius * c2.Transform.Scale.x);
        }

        // This check is an approximation and NOT exact mathematically. Ellipses are weird and hard, no fun ):
        // Here we transform one of the ellipses to a convex polygon and then we check if the circle intersect it
        // the downside with this is that we lose precision in narrow parts 
        return DoCirclePolygonIntersect(c1, c2, c2.CircleAsPoints);
    }

    private static bool DoesPixelOnColliderIntersect(PixelCollider pixel, Collider collider) {
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

    private static bool DoPolygonCollidersIntersect(PolygonCollider p1, PolygonCollider p2) {
        return p1.Triangulation.Any(triangle => DoPolygonAndConvexPolygonIntersect(p2, triangle));
    }

    private static bool DoPolygonAndConvexPolygonIntersect(PolygonCollider polygon, IReadOnlyList<Vector2> polygon2) {
        return polygon.Triangulation.Any(triangle => DoConvexPolygonsIntersect(triangle, polygon2));
    }

    private static bool DoPolygonAndCircleIntersect(PolygonCollider polygon, CircleCollider circle) {
        return polygon.Triangulation.Any(triangle => DoCirclePolygonIntersect(circle, polygon, triangle));
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

    private static bool DoesBoxOnCircleIntersect(CircleCollider c1, BoxCollider c2) {
        return DoCirclePolygonIntersect(c1, c2, c2.WorldCorners);
    }

    private static bool DoSameRotationBoxesIntersect(BoxCollider c1, BoxCollider c2) {
        Vector2 c2BottomLeft = c1.Transform.WorldToLocalMatrix.ConvertPoint(
            c2.Transform.LocalToWorldMatrix.ConvertPoint(c2.BottomLeftLocal)
        );
        Vector2 c2TopRight = c1.Transform.WorldToLocalMatrix.ConvertPoint(
            c2.Transform.LocalToWorldMatrix.ConvertPoint(c2.TopRightLocal)
        );
        return DoRectanglesIntersect(c1.BottomLeftLocal, c1.TopRightLocal, c2BottomLeft, c2TopRight);
    }

    private static bool DoRectanglesIntersect(Vector2 bottomLeft1, Vector2 topRight1, Vector2 bottomLeft2, Vector2 topRight2) {
        return bottomLeft1.x < topRight2.x
               && topRight1.x > bottomLeft2.x
               && bottomLeft1.y < topRight2.y
               && topRight1.y > bottomLeft2.y;
    }
    
    private static bool DoConvexPolygonsIntersect(IReadOnlyList<Vector2> c1Points, IReadOnlyList<Vector2> c2Points) {
        foreach (IReadOnlyList<Vector2> points in new[] { c1Points, c2Points })
        {
            for (int i1 = 0; i1 < points.Count; i1++)
            {
                int i2 = (i1 + 1) % points.Count;
                Vector2 normal = new(points[i2].y - points[i1].y, points[i1].x - points[i2].x);

                FindMinMaxPointsProjectedAlongNormal(c1Points, normal, out float minA, out float maxA);
                FindMinMaxPointsProjectedAlongNormal(c2Points, normal, out float minB, out float maxB);

                if (maxA <= minB || maxB <= minA) {
                    return false;
                }
            }
        }
        return true;
    }
    
    private static void FindMinMaxPointsProjectedAlongNormal(IEnumerable<Vector2> corners, Vector2 normal, out float min, out float max) {
        min = float.MaxValue;
        max = float.MinValue;
        foreach (float projected in corners.Select(p => Vector2.Dot(normal, p))) {
            min = projected < min ? projected : min;
            max = projected > max ? projected : max;
        }
    }

    private static bool DoCirclesIntersect(
        Vector2 c1Center,
        Vector2 c2Center,
        float c1Radius,
        float c2Radius
    ) {
        Vector2 distance = c1Center - c2Center;
        float radiusSum = c1Radius + c2Radius;
        return distance.x * distance.x + distance.y * distance.y <= radiusSum * radiusSum;
    }

    private static bool DoCirclePolygonIntersect(CircleCollider c1, Collider c2, IReadOnlyList<Vector2> c2Points) {
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