using Worms.engine.core.gizmos;
using Worms.engine.core.update.physics;
using Worms.engine.data;

namespace Worms.engine.game_object.components.physics.colliders; 

public class BoxCollider : Collider {
    public Vector2 size;

    public List<Vector2> WorldCorners => GetLocalCorners().Select(c => Transform.LocalToWorldMatrix.ConvertPoint(c)).ToList();

    public Vector2 BottomLeftLocal => GetLocalCorners()[0];
    public Vector2 TopRightLocal => GetLocalCorners()[2];

    public BoxCollider(
        bool isActive, 
        ColliderState state,
        Vector2 size,
        Vector2 offset
    ) : base(isActive, state, offset) {
        this.size = size;
    }

    public override List<Vector2> GetLocalCorners() {
        return new List<Vector2> {
            new(offset.x - size.x / 2, offset.y - size.y / 2),
            new(offset.x - size.x / 2, offset.y + size.y / 2),
            new(offset.x + size.x / 2, offset.y + size.y / 2),
            new(offset.x + size.x / 2, offset.y - size.y / 2)
        };
    }

    public override bool IsPointInside(Vector2 p) {
        p = Transform.WorldToLocalMatrix.ConvertPoint(p);
        return p.x >= BottomLeftLocal.x && p.x <= TopRightLocal.x && p.y >= BottomLeftLocal.y && p.y <= TopRightLocal.y;
    }

    public override ColliderHit? Raycast(Vector2 origin, Vector2 direction) {
        if (IsPointInside(origin)) {
            return null;
        }
        
        origin = Transform.WorldToLocalMatrix.ConvertPoint(origin);
        direction = Transform.WorldToLocalMatrix.ConvertVector(direction);

        if (PhysicsUtils.LineBoxIntersectionWithNormal(GetLocalCorners(), origin, direction, out Tuple<Vector2, Vector2> value)) {
            return new ColliderHit(
                Transform.LocalToWorldMatrix.ConvertPoint(value.Item1),
                Transform.LocalToWorldMatrix.ConvertVector(value.Item2).Normalized
            );
        }

        return null;
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawRectangle(Center, size * Transform.Scale, Transform.Rotation, GIZMO_COLOR);
        base.OnDrawGizmos();
    }
}