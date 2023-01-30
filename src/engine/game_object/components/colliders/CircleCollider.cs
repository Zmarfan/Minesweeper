using Worms.engine.core.gizmos;
using Worms.engine.data;

namespace Worms.engine.game_object.components.colliders; 

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

    public override void OnDrawGizmos() {
        Gizmos.DrawEllipsis(Center, radius * Transform.Scale, Transform.Rotation, GIZMO_COLOR);
    }
}