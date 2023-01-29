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
        return true;
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawRectangle(Center, size * Transform.Scale, Transform.Rotation, GIZMO_COLOR);
    }
}