using Worms.engine.core.gizmos.objects;
using Worms.engine.data;
using Worms.engine.helper;

namespace Worms.engine.core.gizmos; 

public static class Gizmos {
    public static TransformationMatrix matrix;
    public static readonly Queue<GizmosObject> GIZMOS_OBJECTS = new();
    private static Dictionary<string, GizmoIdDetails> _idDetails = new();

    public static void Init(GizmoSettings settings) {
        _idDetails = settings.gizmoIdDetails;
    }
    
    public static void DrawLine(Vector2 from, Vector2 to, string id) {
        AddGizmoWithId(id, c => DrawLine(from, to, c));
    }
    
    public static void DrawLine(Vector2 from, Vector2 to, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmosLine(ToWorld(from), ToWorld(to), color));
    }
    
    public static void DrawRay(Vector2 from, Vector2 direction, string id) {
        AddGizmoWithId(id, c => DrawRay(from, direction, c));
    }
    
    public static void DrawRay(Vector2 from, Vector2 direction, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmosLine(ToWorld(from), ToWorld(from + direction), color));
    }

    public static void DrawIcon(Vector2 center, string id) {
        AddGizmoWithId(id, c => DrawIcon(center, c));
    }
    
    public static void DrawIcon(Vector2 center, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmoIcon(ToWorld(center), color));
    }
    
    public static void DrawCircle(Vector2 center, float radius, string id) {
        AddGizmoWithId(id, c => DrawCircle(center, radius, c));
    }
    
    public static void DrawCircle(Vector2 center, float radius, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmosEllipsis(
            ToWorld(center), 
            radius * matrix.GetScale(), 
            matrix.GetRotation(),
            color
        ));
    }
    
    public static void DrawEllipsis(Vector2 center, Vector2 radius, Rotation rotation, string id) {
        AddGizmoWithId(id, c => DrawEllipsis(center, radius, rotation, c));
    }
    
    public static void DrawEllipsis(Vector2 center, Vector2 radius, Rotation rotation, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmosEllipsis(
            ToWorld(center), 
            radius * matrix.GetScale(), 
            matrix.GetRotation() + rotation,
            color
        ));
    }
    
    public static void DrawRectangle(Vector2 center, Vector2 size, Rotation rotation, string id) {
        AddGizmoWithId(id, c => DrawRectangle(center, size, rotation, c));
    }
    
    public static void DrawRectangle(Vector2 center, Vector2 size, Rotation rotation, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmosRectangle(
            ToWorld(center), 
            size * matrix.GetScale(), 
            matrix.GetRotation() + rotation,
            color
        ));
    }
    
    public static void DrawPolygon(IReadOnlyList<Vector2> points, string id) {
        AddGizmoWithId(id, c => DrawPolygon(points, c));
    }
    
    public static void DrawPolygon(IReadOnlyList<Vector2> points, Color color) {
        int fromIndex = points.Count - 1;
        for (int toIndex = 0; toIndex < points.Count; toIndex++) {
            DrawLine(points[fromIndex], points[toIndex], color);
            fromIndex = toIndex;
        }
    }
    
    public static void DrawPoint(Vector2 point, string id) {
        AddGizmoWithId(id, c => DrawPoint(point, c));
    }
    
    public static void DrawPoint(Vector2 point, Color color) {
        DrawPoints(ListUtils.Of(point), color);
    }

    public static void DrawPoints(IEnumerable<Vector2> points, string id) {
        AddGizmoWithId(id, c => DrawPoints(points, c));
    }
    
    public static void DrawPoints(IEnumerable<Vector2> points, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmosPoints(points.Select(ToWorld), color));
    }

    private static void AddGizmoWithId(string id, Action<Color> gizmoAdder) {
        GetIdDetailsIfPresent(id, out bool show, out Color color);
        if (show) {
            gizmoAdder.Invoke(color);
        }
    }
    
    private static void GetIdDetailsIfPresent(string id, out bool show, out Color color) {
        show = false;
        color = Color.BLACK;
        if (!_idDetails.TryGetValue(id, out GizmoIdDetails? details)) {
            throw new ArgumentException($"There is no registered gizmo details for id: {id}");
        }
        show = details.show;
        color = details.color;
    }

    private static Vector2 ToWorld(Vector2 position) {
        return matrix.ConvertPoint(position);
    }
}