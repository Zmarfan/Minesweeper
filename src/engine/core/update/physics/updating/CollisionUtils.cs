using Worms.engine.core.game_object_handler;
using Worms.engine.data;
using Worms.engine.game_object.components;
using Worms.engine.game_object.components.physics.colliders;

namespace Worms.engine.core.update.physics.updating; 

public static class CollisionUtils {
    public static bool ObjectsCollide(TrackObject obj1, TrackObject obj2, out CollisionData data) {
        Collider c1 = obj1.Collider!;
        Collider c2 = obj2.Collider!;
        data = new CollisionData();
        if (!IntersectUtils.DoBoundingBoxesIntersect(c1, c2)) {
            return false;
        }
        
        Vector2 relativeVelocity = obj2.RigidBody!.velocity - obj1.RigidBody!.velocity;
        return c1 switch {
            BoxCollider box1 when c2 is BoxCollider box2 => DoConvexPolygonsCollide(box1.WorldCorners, box2.WorldCorners, relativeVelocity, out data),
            CircleCollider circle1 when c2 is CircleCollider circle2 => DoesCircleOnCircleCollide(circle1, circle2, relativeVelocity, out data),
            BoxCollider box when c2 is CircleCollider circle => DoCirclePolygonIntersect(circle, box.WorldCorners, relativeVelocity, box.IsPointInside(circle.Center), false, out data),
            CircleCollider circle when c2 is BoxCollider box => DoCirclePolygonIntersect(circle, box.WorldCorners, relativeVelocity, box.IsPointInside(circle.Center), true, out data),
            _ => throw new Exception($"The collider types: {c1} and {c2} are not supported in the physics trigger system!")
        };
    }

    private static bool DoesCircleOnCircleCollide(CircleCollider c1, CircleCollider c2, Vector2 relativeVelocity, out CollisionData data) {
        if (IsScaleUniform(c1) && IsScaleUniform(c2)) {
            return DoCirclesCollide(c1.Center, c2.Center, c1.radius * c1.Transform.Scale.x, c2.radius * c2.Transform.Scale.x, out data);
        }
        
        // This check is an approximation and NOT exact mathematically. Ellipses are weird and hard, no fun ):
        // Here we transform one of the ellipses to a convex polygon and then we check if the circle intersect it
        // the downside with this is that we lose precision in narrow parts 
        return DoCirclePolygonIntersect(c1, c2.CircleAsPoints, relativeVelocity, c2.IsPointInside(c1.Center), true, out data);
    }

    private static bool DoConvexPolygonsCollide(List<Vector2> c1Points, List<Vector2> c2Points, Vector2 relativeVelocity, out CollisionData data) {
        data = new CollisionData(Vector2.Left(), float.MaxValue);
        foreach (List<Vector2> points in new[] { c1Points, c2Points })
        {
            for (int i1 = 0; i1 < points.Count; i1++)
            {
                int i2 = (i1 + 1) % points.Count;
                Vector2 normal = new Vector2(points[i2].y - points[i1].y, points[i1].x - points[i2].x).Normalized;

                ProjectPointsAlongNormal(c1Points, normal, out float minA, out float maxA);
                ProjectPointsAlongNormal(c2Points, normal, out float minB, out float maxB);

                if (maxA <= minB || maxB <= minA) {
                    return false;
                }

                if (Vector2.Dot(relativeVelocity, normal) > 0) {
                    continue;
                }
                
                float penetration = Math.Abs(minB - maxA);
                if (data.penetration > penetration) {
                    data = new CollisionData(normal, penetration);
                }
            }
        }
        return true;
    }

    private static bool DoCirclesCollide(
        Vector2 c1Center,
        Vector2 c2Center,
        float c1Radius,
        float c2Radius,
        out CollisionData data
    ) {
        if (c1Center == c2Center) {
            data = new CollisionData(Vector2.Up(), Math.Max(c1Radius, c2Radius));
            return true;
        }
        
        Vector2 distance = c2Center - c1Center;
        float radiusSum = c1Radius + c2Radius;
        bool intersect = distance.SqrMagnitude < radiusSum * radiusSum;
        if (intersect) {
            data = new CollisionData(distance.Normalized, radiusSum - distance.Magnitude);
            return true;
        }

        data = new CollisionData();
        return false;
    }

    private static bool DoCirclePolygonIntersect(
        CircleCollider c1,
        IReadOnlyList<Vector2> c2Points,
        Vector2 relativeVelocity,
        bool circleInPolygon,
        bool flip,
        out CollisionData data
    ) {
        c2Points = c2Points.Select(p => c1.Transform.WorldToLocalMatrix.ConvertPoint(p)).ToList();

        data = new CollisionData(Vector2.Left(), float.MaxValue);
        if (!CalculateCirclePolygonEdgeCollision(c1, c2Points, relativeVelocity, ref data)) {
            return false;
        }

        if (!CalculateCirclePolygonVertexCollision(c1, c2Points, relativeVelocity, circleInPolygon, ref data)) {
            return false;
        }

        if (flip) {
            data = new CollisionData(data.normal * -1, data.penetration);
        }

        return true;
    }

    private static bool CalculateCirclePolygonEdgeCollision(
        CircleCollider c1,
        IReadOnlyList<Vector2> c2Points,
        Vector2 relativeVelocity,
        ref CollisionData data
    ) {
        for (int i1 = 0; i1 < c2Points.Count; i1++)
        {
            int i2 = (i1 + 1) % c2Points.Count;
            Vector2 normal = new Vector2(c2Points[i2].y - c2Points[i1].y, c2Points[i1].x - c2Points[i2].x).Normalized;

            ProjectPointsAlongNormal(c2Points, normal, out float minA, out float maxA);
            ProjectCircleAlongNormal(c1.offset, c1.radius, normal, out float minB, out float maxB);

            if (maxA <= minB || maxB <= minA) {
                return false;
            }

            if (Vector2.Dot(relativeVelocity, normal) > 0) {
                continue;
            }
                
            float penetration = Math.Abs(minB - maxA);
            if (data.penetration > penetration) {
                data = new CollisionData(normal, penetration);
            }
        }
        return true;
    }

    private static bool CalculateCirclePolygonVertexCollision(
        CircleCollider c1, 
        IReadOnlyList<Vector2> c2Points,
        Vector2 relativeVelocity,
        bool circleInPolygon,
        ref CollisionData data
    ) {
        foreach (Vector2 point in c2Points) {
            Vector2 normal = (c1.offset - point).Normalized * (circleInPolygon ? -1 : 1);

            ProjectPointsAlongNormal(c2Points, normal, out float minA, out float maxA);
            ProjectCircleAlongNormal(c1.offset, c1.radius, normal, out float minB, out float maxB);

            if (maxA <= minB || maxB <= minA) {
                return false;
            }

            if (Vector2.Dot(relativeVelocity, normal) > 0) {
                continue;
            }
                
            float penetration = Math.Abs(minB - maxA);
            if (data.penetration > penetration) {
                data = new CollisionData(normal, penetration);
            }
        }

        return true;
    }
    
    private static void ProjectPointsAlongNormal(IEnumerable<Vector2> corners, Vector2 normal, out float min, out float max) {
        min = float.MaxValue;
        max = float.MinValue;
        foreach (float projected in corners.Select(p => Vector2.Dot(normal, p))) {
            min = projected < min ? projected : min;
            max = projected > max ? projected : max;
        }
    }
    
    private static void ProjectCircleAlongNormal(Vector2 center, float radius, Vector2 normal, out float min, out float max) {
        Vector2 radiusOffset = normal * radius;
        min = Vector2.Dot(center - radiusOffset, normal);
        max = Vector2.Dot(center + radiusOffset, normal);
    }
    
    private static bool IsScaleUniform(Component c) {
        return Math.Abs(c.Transform.Scale.x - c.Transform.Scale.y) < 0.001f;
    }
}