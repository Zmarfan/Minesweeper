namespace GameEngine.engine.core.gizmos; 

public class GizmoSettings {
    public const string COLLIDER_NAME = "internal_gizmo_collider";
    public const string TRIGGER_NAME = "internal_gizmo_trigger";
    public const string BOUNDING_BOX_NAME = "internal_gizmo_bounding_box";
    public const string CIRCLE_POLYGON_NAME = "internal_gizmo_circle_polygon";
    public const string TEXT_AREA_NAME = "internal_gizmo_text_area";
    public const string POLYGON_TRIANGLES_NAME = "internal_gizmo_polygon_triangles";
    
    public Dictionary<string, GizmoIdDetails> gizmoIdDetails;

    public GizmoSettings(Dictionary<string, GizmoIdDetails> gizmoIdDetails) {
        this.gizmoIdDetails = gizmoIdDetails;
    }
}