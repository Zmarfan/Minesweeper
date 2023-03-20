using GameEngine.engine.data;

namespace GameEngine.engine.core.gizmos; 

public record GizmoIdDetails(bool show, Color color) {
    public bool show = show;
    public Color color = color;
}