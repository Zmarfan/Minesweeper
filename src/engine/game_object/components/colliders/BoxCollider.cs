using Worms.engine.core.gizmos;
using Worms.engine.data;

namespace Worms.engine.game_object.components.colliders; 

public class BoxCollider : Collider {
    public Vector2 size;
    private Vector2 Center => Transform.Position + Transform.LocalToWorldMatrix.ConvertVector(offset);
    
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
        float minX = offset.x - size.x / 2;
        float maxX = offset.x + size.x / 2;
        float minY = offset.y - size.y / 2;
        float maxY = offset.y + size.y / 2;
        return p.x >= minX && p.x <= maxX && p.y >= minY && p.y <= maxY;
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawRectangle(Center, size * Transform.Scale, Transform.Rotation, GIZMO_COLOR);
    }
}