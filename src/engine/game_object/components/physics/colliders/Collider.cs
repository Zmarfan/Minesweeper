using Worms.engine.core.gizmos;
using Worms.engine.core.update.physics;
using Worms.engine.data;

namespace Worms.engine.game_object.components.physics.colliders; 

public abstract class Collider : ToggleComponent {
    protected static readonly Color COLLIDER_GIZMO_COLOR = new(0.1059f, 0.949f, 0.3294f);
    private static readonly Color BOUNDING_BOX_GIZMO_COLOR = new(0.25f, 0.67f, 0.9f);

    public Vector2 Center => Transform.LocalToWorldMatrix.ConvertPoint(offset);
    
    public ColliderState state;
    public Vector2 offset;

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

        Gizmos.DrawLine(new Vector2(minX, minY), new Vector2(minX, maxY), BOUNDING_BOX_GIZMO_COLOR);
        Gizmos.DrawLine(new Vector2(minX, minY), new Vector2(maxX, minY), BOUNDING_BOX_GIZMO_COLOR);
        Gizmos.DrawLine(new Vector2(maxX, maxY), new Vector2(minX, maxY), BOUNDING_BOX_GIZMO_COLOR);
        Gizmos.DrawLine(new Vector2(maxX, maxY), new Vector2(maxX, minY), BOUNDING_BOX_GIZMO_COLOR);
    }
}