using Worms.engine.core.gizmos;
using Worms.engine.core.update;
using Worms.engine.data;

namespace Worms.engine.game_object.components.colliders; 

public class BoxCollider : Collider {
    public Vector2 size;

    private Vector2 TopLeft => new(offset.x - size.x / 2, offset.y + size.y / 2);
    private Vector2 TopRight => new(offset.x + size.x / 2, offset.y + size.y / 2);
    private Vector2 BottomLeft => new(offset.x - size.x / 2, offset.y - size.y / 2);
    private Vector2 BottomRight => new(offset.x + size.x / 2, offset.y - size.y / 2);

    public BoxCollider(
        bool isActive, 
        bool isTrigger,
        Vector2 size,
        Vector2 offset
    ) : base(isActive, isTrigger, offset) {
        this.size = size;
    }

    public override bool IsPointInside(Vector2 p) {
        p = Transform.WorldToLocalMatrix.ConvertPoint(p);
        return p.x >= BottomLeft.x && p.x <= TopRight.x && p.y >= BottomLeft.y && p.y <= TopRight.y;
    }

    public override ColliderHit? Raycast(Vector2 origin, Vector2 direction) {
        if (IsPointInside(origin)) {
            return null;
        }
        
        origin = Transform.WorldToLocalMatrix.ConvertPoint(origin);
        direction = Transform.WorldToLocalMatrix.ConvertVector(direction);

        List<Tuple<Vector2, Vector2>> pointsWithNormals = CalculateIntersectionPoints(origin, direction);
        if (pointsWithNormals.Count == 0) {
            return null;
        }

        Tuple<Vector2, Vector2> bestHit = pointsWithNormals.MinBy(p => (p.Item1 - origin).SqrMagnitude)!;
        return new ColliderHit(
            Transform.LocalToWorldMatrix.ConvertPoint(bestHit.Item1),
            Transform.LocalToWorldMatrix.ConvertVector(bestHit.Item2).Normalized
        );
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawRectangle(Center, size * Transform.Scale, Transform.Rotation, GIZMO_COLOR);
    }
    
    private List<Tuple<Vector2, Vector2>> CalculateIntersectionPoints(Vector2 origin, Vector2 direction) {
        List<Tuple<Vector2, Vector2>> points = new();
        if (PhysicsUtils.LineIntersection(origin, direction, BottomLeft, TopLeft, out Vector2? p1)) {
            points.Add(new Tuple<Vector2, Vector2>(p1!.Value, Vector2.Left()));
        }
        if (PhysicsUtils.LineIntersection(origin, direction, BottomLeft, BottomRight, out Vector2? p2)) {
            points.Add(new Tuple<Vector2, Vector2>(p2!.Value, Vector2.Down()));
        }
        if (PhysicsUtils.LineIntersection(origin, direction, TopLeft, TopRight, out Vector2? p3)) {
            points.Add(new Tuple<Vector2, Vector2>(p3!.Value, Vector2.Up()));
        }
        if (PhysicsUtils.LineIntersection(origin, direction, BottomRight, TopRight, out Vector2? p4)) {
            points.Add(new Tuple<Vector2, Vector2>(p4!.Value, Vector2.Right()));
        }

        return points;
    }
}