using Worms.engine.core.gizmos;
using Worms.engine.data;

namespace Worms.engine.game_object.components.colliders; 

public class PixelCollider : Collider {
    private readonly HashSet<Vector2> _pixels;

    public PixelCollider(
        bool isActive,
        IEnumerable<Vector2> pixels,
        bool isTrigger,
        Vector2 offset
    ) : base(isActive, isTrigger, new Vector2((int)offset.x, (int)offset.y)) {
        _pixels = pixels.Select(p => new Vector2((int)p.x, (int)p.y)).ToHashSet();
    }

    public override bool IsPointInside(Vector2 p) {
        p = Transform.WorldToLocalMatrix.ConvertPoint(p);
        return _pixels.Contains(new Vector2((int)Math.Round(p.x) - offset.x, (int)Math.Round(p.y) - offset.y));
    }

    public override void OnDrawGizmos() {
        foreach (Vector2 point in _pixels) {
            Gizmos.DrawPoint(Transform.LocalToWorldMatrix.ConvertPoint(point + offset), GIZMO_COLOR);
        }
    }
}