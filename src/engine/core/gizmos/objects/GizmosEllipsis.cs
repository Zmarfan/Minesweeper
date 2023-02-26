using Worms.engine.core.renderer;
using Worms.engine.data;

namespace Worms.engine.core.gizmos.objects; 

public class GizmosEllipsis : GizmosObject {
    private readonly Vector2 _center;
    private readonly Vector2 _radius;
    private readonly Rotation _rotation;

    public GizmosEllipsis(Vector2 center, Vector2 radius, Rotation rotation, Color color) : base(color) {
        _center = center;
        _radius = radius;
        _rotation = rotation;
    }

    public override void Render(nint renderer, TransformationMatrix worldToScreenMatrix) {
        Vector2 radius = worldToScreenMatrix.ConvertVector(_radius);
        radius.Abs();
        GizmoRendererHelper.DrawEllipse(
            renderer,
            worldToScreenMatrix.ConvertPoint(_center),
            radius,
            _rotation
        );
    }
}