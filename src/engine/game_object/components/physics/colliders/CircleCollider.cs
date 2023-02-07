using Worms.engine.core.gizmos;
using Worms.engine.core.update.physics;
using Worms.engine.data;

namespace Worms.engine.game_object.components.physics.colliders; 

public class CircleCollider : Collider {
    public float radius;
    
    public CircleCollider(
        bool isActive, 
        bool isTrigger,
        float radius,
        Vector2 offset
    ) : base(isActive, isTrigger, offset) {
        this.radius = radius;
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
        Gizmos.DrawEllipsis(Center, radius * Transform.Scale, Transform.Rotation, GIZMO_COLOR);
    }
}