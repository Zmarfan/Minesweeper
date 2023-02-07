using Worms.engine.core.gizmos;
using Worms.engine.core.update.physics;
using Worms.engine.data;

namespace Worms.engine.game_object.components.physics.colliders; 

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

        if (PhysicsUtils.LineBoxIntersectionWithNormal(BottomLeft, TopLeft, TopRight, BottomRight, origin, direction, out Tuple<Vector2, Vector2> value)) {
            return new ColliderHit(
                Transform.LocalToWorldMatrix.ConvertPoint(value.Item1),
                Transform.LocalToWorldMatrix.ConvertVector(value.Item2).Normalized
            );
        }

        return null;
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawRectangle(Center, size * Transform.Scale, Transform.Rotation, GIZMO_COLOR);
    }
}