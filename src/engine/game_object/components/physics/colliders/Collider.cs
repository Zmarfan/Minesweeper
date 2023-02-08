using Worms.engine.core.gizmos;
using Worms.engine.core.update.physics;
using Worms.engine.data;

namespace Worms.engine.game_object.components.physics.colliders; 

public abstract class Collider : ToggleComponent {
    public static readonly Color GIZMO_COLOR = new(0.1059f, 0.949f, 0.3294f);

        public Vector2 Center => Transform.LocalToWorldMatrix.ConvertPoint(offset);
    
    public ColliderState state;
    public Vector2 offset;

    protected Collider(bool isActive, ColliderState state, Vector2 offset) : base(isActive) {
        this.state = state;
        this.offset = offset;
    }

    public abstract Tuple<Vector2, Vector2> GetWorldBoundingBox();
    public abstract bool IsPointInside(Vector2 p);
    public abstract ColliderHit? Raycast(Vector2 origin, Vector2 direction);

    public override void OnDrawGizmos() {
        Tuple<Vector2, Vector2> boundingBox = GetWorldBoundingBox();
        Gizmos.DrawLine(boundingBox.Item1, boundingBox.Item2, new Color(0.25f, 0.67f, 0.9f));
    }
}