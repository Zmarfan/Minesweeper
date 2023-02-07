using Worms.engine.data;
using Worms.engine.game_object.components.physics.colliders;

namespace Worms.engine.core.update.physics; 

public static class TriggerIntersectUtils {
    public static bool DoesBoxOnBoxOverlap(BoxCollider c1, BoxCollider c2) {
        foreach (BoxCollider box in new[] { c1, c2 })
        {
            for (int i1 = 0; i1 < box.WorldCorners.Count; i1++)
            {
                int i2 = (i1 + 1) % box.WorldCorners.Count;
                Vector2 p1 = box.WorldCorners[i1];
                Vector2 p2 = box.WorldCorners[i2];

                Vector2 normal = new(p2.y - p1.y, p1.x - p2.x);

                ProjectBoxAlongNormal(c1.WorldCorners, normal, out float minA, out float maxA);
                ProjectBoxAlongNormal(c2.WorldCorners, normal, out float minB, out float maxB);

                if (maxA < minB || maxB < minA) {
                    return false;
                }
            }
        }
        return true;
    }

    private static void ProjectBoxAlongNormal(IEnumerable<Vector2> corners, Vector2 normal, out float min, out float max) {
        min = float.MaxValue;
        max = float.MinValue;
        foreach (float projected in corners.Select(p => Vector2.Dot(normal, p))) {
            min = projected < min ? projected : min;
            max = projected > max ? projected : max;
        }
    }

    public static bool DoesCircleOnCircleOverlap(Collider c1, Collider c2) {
        return false;
    }

    public static bool DoesPixelOnPixelOverlap(Collider c1, Collider c2) {
        return false;
    }

    public static bool DoesBoxOnCircleOverlap(Collider c1, Collider c2) {
        return false;
    }

    public static bool DoesBoxOnPixelOverlap(Collider c1, Collider c2) {
        return false;
    }

    public static bool DoesCircleOnPixelOverlap(Collider c1, Collider c2) {
        return false;
    }
}