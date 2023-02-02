using Worms.engine.core.gizmos;
using Worms.engine.data;

namespace Worms.engine.game_object.components.colliders; 

public class PixelCollider : Collider {
    public HashSet<Vector2> Pixels {
        get => _pixels;
        set {
            _pixels = value.Select(p => new Vector2((int)p.x, (int)p.y)).ToHashSet();
        }
    }

    private HashSet<Vector2> _pixels = new();

    public PixelCollider(
        bool isActive,
        IEnumerable<Vector2> pixels,
        bool isTrigger,
        Vector2 offset
    ) : base(isActive, isTrigger, new Vector2((int)offset.x, (int)offset.y)) {
        Pixels = pixels.ToHashSet();
    }

    public override bool IsPointInside(Vector2 p) {
        p = Transform.WorldToLocalMatrix.ConvertPoint(p);
        return _pixels.Contains(new Vector2((int)Math.Round(p.x) - offset.x, (int)Math.Round(p.y) - offset.y));
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawPoints(_pixels.Select(p => Transform.LocalToWorldMatrix.ConvertPoint(p + offset)), GIZMO_COLOR);
    }
}