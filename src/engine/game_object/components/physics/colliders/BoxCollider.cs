using Worms.engine.core.gizmos;
using Worms.engine.core.update.physics;
using Worms.engine.data;

namespace Worms.engine.game_object.components.physics.colliders; 

public class BoxCollider : Collider {
    public Vector2 size;

    public List<Vector2> WorldCorners => LocalCorners.Select(c => Transform.LocalToWorldMatrix.ConvertPoint(c)).ToList();
    private List<Vector2> LocalCorners => new() {
        new Vector2(offset.x - size.x / 2, offset.y - size.y / 2),
        new Vector2(offset.x - size.x / 2, offset.y + size.y / 2),
        new Vector2(offset.x + size.x / 2, offset.y + size.y / 2),
        new Vector2(offset.x + size.x / 2, offset.y - size.y / 2)
    };

    public Vector2 BottomLeftLocal => LocalCorners[0];
    public Vector2 TopRightLocal => LocalCorners[2];

    public BoxCollider(
        bool isActive, 
        ColliderState state,
        Vector2 size,
        Vector2 offset
    ) : base(isActive, state, offset) {
        this.size = size;
    }

    public override Tuple<Vector2, Vector2> GetWorldBoundingBox() {
        List<Vector2> worldCorners = WorldCorners;
        float minX = worldCorners.MinBy(p => p.x).x;
        float minY = worldCorners.MinBy(p => p.y).y;
        float maxX = worldCorners.MaxBy(p => p.x).x;
        float maxY = worldCorners.MaxBy(p => p.y).y;
        return new Tuple<Vector2, Vector2>(new Vector2(minX, minY), new Vector2(maxX, maxY));
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

        if (PhysicsUtils.LineBoxIntersectionWithNormal(LocalCorners, origin, direction, out Tuple<Vector2, Vector2> value)) {
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