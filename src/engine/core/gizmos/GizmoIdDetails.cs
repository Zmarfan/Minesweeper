using Worms.engine.data;

namespace Worms.engine.core.gizmos; 

public record GizmoIdDetails(bool show, Color color) {
    public bool show = show;
    public Color color = color;
}