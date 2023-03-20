namespace GameEngine.engine.core.gizmos; 

public class GizmoSettings {
    internal const string COLLIDER_NAME = "internal_gizmo_collider";
    internal const string TRIGGER_NAME = "internal_gizmo_trigger";
    internal const string BOUNDING_BOX_NAME = "internal_gizmo_bounding_box";
    internal const string CIRCLE_POLYGON_NAME = "internal_gizmo_circle_polygon";
    internal const string TEXT_AREA_NAME = "internal_gizmo_text_area";
    internal const string POLYGON_TRIANGLES_NAME = "internal_gizmo_polygon_triangles";
    
    public readonly Dictionary<string, GizmoIdDetails> gizmoIdDetails;

    public GizmoSettings(Dictionary<string, GizmoIdDetails> gizmoIdDetails) {
        this.gizmoIdDetails = gizmoIdDetails;
    }
}