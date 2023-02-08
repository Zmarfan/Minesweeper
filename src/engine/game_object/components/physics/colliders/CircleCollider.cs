using Worms.engine.core.gizmos;
using Worms.engine.core.update.physics;
using Worms.engine.data;

namespace Worms.engine.game_object.components.physics.colliders; 

public class CircleCollider : Collider {
    public float radius;
    
    public CircleCollider(
        bool isActive, 
        ColliderState state,
        float radius,
        Vector2 offset
    ) : base(isActive, state, offset) {
        this.radius = radius;
    }

    public List<Vector2> GetCircleAsPoints(int amount) {
        List<Vector2> points = new();
        
        float theta = (float)(Math.PI * 2 / amount);
        for (int i = 0; i < amount; i++) {
            Rotation angle = Rotation.FromRadians(theta * i);
            Vector2 point = offset + new Vector2((float)(radius * Math.Cos(angle.Radians)), (float)(radius * Math.Sin(angle.Radians)));
            points.Add(Transform.LocalToWorldMatrix.ConvertPoint(point));
        }

        return points;
    }

    public override Tuple<Vector2, Vector2> GetWorldBoundingBox() {
        Vector2 bottomLeft = Transform.LocalToWorldMatrix.ConvertPoint(offset + new Vector2(-radius, -radius));
        Vector2 topRight = Transform.LocalToWorldMatrix.ConvertPoint(offset + new Vector2(radius, radius));
        return new Tuple<Vector2, Vector2>(bottomLeft, topRight);
    }

    public override bool IsPointInside(Vector2 p) {
        return (Transform.WorldToLocalMatrix.ConvertPoint(p) - offset).SqrMagnitude <= radius * radius;
    }

    public override ColliderHit? Raycast(Vector2 origin, Vector2 direction) {
        origin = Transform.WorldToLocalMatrix.ConvertPoint(origin);
        direction = Transform.WorldToLocalMatrix.ConvertVector(direction);
        if (PhysicsUtils.LineCircleIntersection(origin, direction, offset, radius, out Vector2 intersection)) {
            return new ColliderHit(
                Transform.LocalToWorldMatrix.ConvertPoint(intersection),
                Transform.LocalToWorldMatrix.ConvertVector(intersection - offset).Normalized
            );
        }

        return null;
    }

    public override void OnDrawGizmos() {
        List<Vector2> points = GetCircleAsPoints(15);
        int from = points.Count - 1;
        for (int i = 0; i < points.Count; i++) {
            Gizmos.DrawLine(points[from], points[i], Color.BLUE);
            from = i;
        }
        
        Gizmos.DrawEllipsis(Center, radius * Transform.Scale, Transform.Rotation, GIZMO_COLOR);
        base.OnDrawGizmos();
    }
}