using GameEngine.engine.core.gizmos.objects;
using GameEngine.engine.data;
using GameEngine.engine.helper;

namespace GameEngine.engine.core.gizmos; 

public static class Gizmos {
    public static TransformationMatrix matrix;
    internal static readonly Queue<IGizmosObject> GIZMOS_OBJECTS = new();
    private static Dictionary<string, GizmoIdDetails> _idDetails = new();

    internal static void Init(GizmoSettings settings) {
        _idDetails = settings.gizmoIdDetails;
    }
    
    public static void DrawLine(Vector2 from, Vector2 to, string id) {
        GetIdDetailsIfPresent(id, out bool show, out Color color);
        if (show) {
            DrawLine(from, to, color);
        }
    }
    
    public static void DrawLine(Vector2 from, Vector2 to, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmosLine(ToWorld(from), ToWorld(to), color));
    }
    
    public static void DrawRay(Vector2 from, Vector2 direction, string id) {
        GetIdDetailsIfPresent(id, out bool show, out Color color);
        if (show) {
            DrawRay(from, direction, color);
        }
    }
    
    public static void DrawRay(Vector2 from, Vector2 direction, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmosLine(ToWorld(from), ToWorld(from + direction), color));
    }

    public static void DrawIcon(Vector2 center, string id) {
        GetIdDetailsIfPresent(id, out bool show, out Color color);
        if (show) {
            DrawIcon(center, color);
        }
    }
    
    public static void DrawIcon(Vector2 center, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmoIcon(ToWorld(center), color));
    }
    
    public static void DrawCircle(Vector2 center, float radius, string id) {
        GetIdDetailsIfPresent(id, out bool show, out Color color);
        if (show) {
            DrawCircle(center, radius, color);
        }
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
        GetIdDetailsIfPresent(id, out bool show, out Color color);
        if (show) {
            DrawEllipsis(center, radius, rotation, color);
        }
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
        GetIdDetailsIfPresent(id, out bool show, out Color color);
        if (show) {
            DrawRectangle(center, size, rotation, color);
        }
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
        GetIdDetailsIfPresent(id, out bool show, out Color color);
        if (show) {
            DrawPolygon(points, color);
        }
    }
    
    public static void DrawPolygon(IReadOnlyList<Vector2> points, Color color) {
        int fromIndex = points.Count - 1;
        for (int toIndex = 0; toIndex < points.Count; toIndex++) {
            DrawLine(points[fromIndex], points[toIndex], color);
            fromIndex = toIndex;
        }
    }
    
    public static void DrawPoint(Vector2 point, string id) {
        GetIdDetailsIfPresent(id, out bool show, out Color color);
        if (show) {
            DrawPoint(point, color);
        }
    }
    
    public static void DrawPoint(Vector2 point, Color color) {
        DrawPoints(ListUtils.Of(point), color);
    }

    public static void DrawPoints(IEnumerable<Vector2> points, string id) {
        GetIdDetailsIfPresent(id, out bool show, out Color color);
        if (show) {
            DrawPoints(points, color);
        }
    }
    
    public static void DrawPoints(IEnumerable<Vector2> points, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmosPoints(points.Select(ToWorld), color));
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