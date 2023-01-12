using Worms.engine.data;

namespace Worms.engine.core.gizmos; 

public static class Gizmos {
    public static TransformationMatrix matrix;
    public static readonly Queue<GizmosObject> GIZMOS_OBJECTS = new();

    public static void DrawLine(Vector2 from, Vector2 to, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmosLine(ToWorld(from), ToWorld(to), color));
    }
    
    public static void DrawCircle(Vector2 center, float radius, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmosEllipsis(
            ToWorld(center), 
            radius * matrix.GetScale(), matrix.GetRotation(),
            color
        ));
    }
    
    public static void DrawEllipsis(Vector2 center, Vector2 radius, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmosEllipsis(
            ToWorld(center), 
            radius * matrix.GetScale(), matrix.GetRotation(),
            color
        ));
    }

    private static Vector2 ToWorld(Vector2 position) {
        return matrix.ConvertPoint(position);
    }
}