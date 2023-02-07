using Worms.engine.game_object.components.physics.colliders;

namespace Worms.engine.core.update.physics; 

public static class TriggerIntersectUtils {
    public static bool DoesBoxOnBoxOverlap(Collider c1, Collider c2) {
        return false;
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