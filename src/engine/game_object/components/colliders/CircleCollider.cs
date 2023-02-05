using Worms.engine.core.gizmos;
using Worms.engine.core.update;
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

    public override Vector2? Raycast(Vector2 origin, Vector2 direction) {
        origin = Transform.WorldToLocalMatrix.ConvertPoint(origin);
        direction = Transform.WorldToLocalMatrix.ConvertVector(direction);

        Vector2 toCircle = origin - offset;
        float a = Vector2.Dot(direction, direction);
        float b = 2 * Vector2.Dot(toCircle, direction);
        float c = Vector2.Dot(toCircle, toCircle) - radius * radius;

        float det = b * b - 4 * a * c;
        
        if (det <= 0) {
            return null;
        }
        float t = (float)(-b - Math.Sqrt(det)) / (2 * a);
        if (t >= 0) {
            return Transform.LocalToWorldMatrix.ConvertPoint(origin + direction * t);
        }

        return null;
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawEllipsis(Center, radius * Transform.Scale, Transform.Rotation, GIZMO_COLOR);
    }
}