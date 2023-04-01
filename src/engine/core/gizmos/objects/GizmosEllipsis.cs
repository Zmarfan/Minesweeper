using GameEngine.engine.core.renderer;
using GameEngine.engine.data;
using Color = GameEngine.engine.data.Color;

namespace GameEngine.engine.core.gizmos.objects; 

internal readonly struct GizmosEllipsis : IGizmosObject {
    private readonly Color _color;
    private readonly Vector2 _center;
    private readonly Vector2 _radius;
    private readonly Rotation _rotation;

    public GizmosEllipsis(Vector2 center, Vector2 radius, Rotation rotation, Color color) {
        _color = color;
        _center = center;
        _radius = radius;
        _rotation = rotation;
    }

    public Color GetColor() {
        return _color;
    }

    public void Render(nint renderer, TransformationMatrix worldToScreenMatrix) {
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