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

    public override Vector2? Raycast(Vector2 origin, Vector2 direction) {
        if (IsPointInside(origin)) {
            return null;
        }
        
        origin = Transform.WorldToLocalMatrix.ConvertPoint(origin);
        direction = Transform.WorldToLocalMatrix.ConvertVector(direction);

        List<Vector2> intersectionPoints = CalculateIntersectionPoints(origin, direction);
        if (intersectionPoints.Count == 0) {
            return null;
        }

        return Transform.LocalToWorldMatrix.ConvertPoint(intersectionPoints.MinBy(p => (p - origin).SqrMagnitude));
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawRectangle(Center, size * Transform.Scale, Transform.Rotation, GIZMO_COLOR);
    }
    
    private List<Vector2> CalculateIntersectionPoints(Vector2 origin, Vector2 direction) {
        List<Vector2> points = new();
        if (PhysicsUtils.LineIntersection(origin, direction, BottomLeft, TopLeft, out Vector2? p1)) {
            points.Add(p1!.Value);
        }
        if (PhysicsUtils.LineIntersection(origin, direction, BottomLeft, BottomRight, out Vector2? p2)) {
            points.Add(p2!.Value);
        }
        if (PhysicsUtils.LineIntersection(origin, direction, TopLeft, TopRight, out Vector2? p3)) {
            points.Add(p3!.Value);
        }
        if (PhysicsUtils.LineIntersection(origin, direction, BottomRight, TopRight, out Vector2? p4)) {
            points.Add(p4!.Value);
        }

        return points;
    }
}