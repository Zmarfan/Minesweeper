using Worms.engine.data;

namespace Worms.engine.core.gizmos; 

public static class Gizmos {
    public static TransformationMatrix matrix;
    public static readonly Queue<GizmosObject> GIZMOS_OBJECTS = new();

    public static void DrawLine(Vector2 from, Vector2 to, Color color) {
        GIZMOS_OBJECTS.Enqueue(new GizmosLine(ToWorldSpace(from), ToWorldSpace(to), color));
    }

    private static Vector2 ToWorldSpace(Vector2 position) {
        return matrix.ConvertLocalPoint(position);
    }
}