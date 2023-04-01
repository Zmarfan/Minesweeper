using GameEngine.engine.data;
using Color = GameEngine.engine.data.Color;

namespace GameEngine.engine.core.gizmos; 

public class GizmoSettingsBuilder {
    private readonly Dictionary<string, GizmoIdDetails> _gizmoIdDetails = new() {
        { GizmoSettings.COLLIDER_NAME, new GizmoIdDetails(true, new Color(0.1059f, 0.949f, 0.3294f)) },
        { GizmoSettings.TRIGGER_NAME, new GizmoIdDetails(true, new Color(0.1059f, 0.649f, 0.3294f)) },
        { GizmoSettings.BOUNDING_BOX_NAME, new GizmoIdDetails(true, new Color(0.25f, 0.67f, 0.9f)) },
        { GizmoSettings.TEXT_AREA_NAME, new GizmoIdDetails(true, new Color(0.85f, 0.2f, 0.2f)) },
        { GizmoSettings.CIRCLE_POLYGON_NAME, new GizmoIdDetails(false, new Color(0.2f, 0.2f, 0.85f)) },
        { GizmoSettings.POLYGON_TRIANGLES_NAME, new GizmoIdDetails(false, new Color(0.5059f, 0.949f, 0.3294f)) },
    };

    public static GizmoSettingsBuilder Builder() {
        return new GizmoSettingsBuilder();
    }

    public GizmoSettings Build() {
        return new GizmoSettings(_gizmoIdDetails);
    }

    public GizmoSettingsBuilder ShowColliders(bool show) {
        _gizmoIdDetails[GizmoSettings.COLLIDER_NAME].show = show;
        return this;
    }
    
    public GizmoSettingsBuilder SetCollidersColor(Color color) {
        _gizmoIdDetails[GizmoSettings.COLLIDER_NAME].color = color;
        return this;
    }
    
    public GizmoSettingsBuilder ShowTriggers(bool show) {
        _gizmoIdDetails[GizmoSettings.TRIGGER_NAME].show = show;
        return this;
    }
    
    public GizmoSettingsBuilder SetTriggersColor(Color color) {
        _gizmoIdDetails[GizmoSettings.TRIGGER_NAME].color = color;
        return this;
    }
    
    public GizmoSettingsBuilder ShowBoundingBoxes(bool show) {
        _gizmoIdDetails[GizmoSettings.BOUNDING_BOX_NAME].show = show;
        return this;
    }
    
    public GizmoSettingsBuilder SetBoundingBoxesColor(Color color) {
        _gizmoIdDetails[GizmoSettings.BOUNDING_BOX_NAME].color = color;
        return this;
    }
    
    public GizmoSettingsBuilder ShowTextAreas(bool show) {
        _gizmoIdDetails[GizmoSettings.TEXT_AREA_NAME].show = show;
        return this;
    }
    
    public GizmoSettingsBuilder SetTextAreasColor(Color color) {
        _gizmoIdDetails[GizmoSettings.TEXT_AREA_NAME].color = color;
        return this;
    }
    
    public GizmoSettingsBuilder ShowCirclePolygons(bool show) {
        _gizmoIdDetails[GizmoSettings.CIRCLE_POLYGON_NAME].show = show;
        return this;
    }
    
    public GizmoSettingsBuilder SetCirclePolygonsColor(Color color) {
        _gizmoIdDetails[GizmoSettings.CIRCLE_POLYGON_NAME].color = color;
        return this;
    }
    
    public GizmoSettingsBuilder ShowPolygonTriangles(bool show) {
        _gizmoIdDetails[GizmoSettings.POLYGON_TRIANGLES_NAME].show = show;
        return this;
    }
    
    public GizmoSettingsBuilder SetPolygonTrianglesColor(Color color) {
        _gizmoIdDetails[GizmoSettings.POLYGON_TRIANGLES_NAME].color = color;
        return this;
    }

    
    public GizmoSettingsBuilder AddGizmoDetails(string id, Color color, bool show = true) {
        _gizmoIdDetails.Add(id, new GizmoIdDetails(show, color));
        _gizmoIdDetails[GizmoSettings.CIRCLE_POLYGON_NAME].color = color;
        return this;
    }
}