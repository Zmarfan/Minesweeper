using Worms.engine.core.renderer;
using Worms.engine.data;

namespace Worms.engine.core.gizmos; 

public class GizmosEllipsis : GizmosObject {
    private readonly Vector2 _center;
    private readonly Vector2 _radius;
    private readonly Rotation _rotation;

    public GizmosEllipsis(Vector2 center, Vector2 radius, Rotation rotation, Color color) : base(color) {
        _center = center;
        _radius = radius;
        _rotation = rotation;
    }

    public override void Render(IntPtr renderer, GameSettings settings) {
        RendererHelper.DrawEllipse(
            renderer,
            WorldToScreenCalculator.WorldToScreenPosition(_center, settings),
            WorldToScreenCalculator.WorldToScreenVector(_radius, settings),
            _rotation
        );
    }
}