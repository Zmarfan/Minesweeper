using GameEngine.engine.core.gizmos;
using GameEngine.engine.core.update.physics;
using GameEngine.engine.data;

namespace GameEngine.engine.game_object.components.physics.colliders; 

public abstract class Collider : ToggleComponent {
    public Vector2 Center => Transform.LocalToWorldMatrix.ConvertPoint(offset);
    
    public ColliderState state;
    public Vector2 offset;

    protected string GizmoId => state is ColliderState.TRIGGER or ColliderState.MOUSE_TRIGGER ? GizmoSettings.TRIGGER_NAME : GizmoSettings.COLLIDER_NAME;
    
    protected readonly Vector2[] localCorners = new Vector2[4];

    protected Collider(bool isActive, ColliderState state, Vector2 offset) : base(isActive) {
        this.state = state;
        this.offset = offset;
    }

    public abstract Vector2[] GetLocalCorners();
    public abstract bool IsPointInside(Vector2 p);
    public abstract ColliderHit? Raycast(Vector2 origin, Vector2 direction);

    public override void OnDrawGizmos() {
        DrawBoundingBox();
    }

    private void DrawBoundingBox() {
        List<Vector2> corners = GetLocalCorners().Select(c => Transform.LocalToWorldMatrix.ConvertPoint(c)).ToList();
        float minX = corners.MinBy(c => c.x).x;
        float minY = corners.MinBy(c => c.y).y;
        float maxX = corners.MaxBy(c => c.x).x;
        float maxY = corners.MaxBy(c => c.y).y;

        Vector2 size = new(maxX - minX, maxY - minY);
        Gizmos.DrawRectangle(new Vector2(minX, minY) + size / 2, size, Rotation.Identity(), GizmoSettings.BOUNDING_BOX_NAME);
    }
}