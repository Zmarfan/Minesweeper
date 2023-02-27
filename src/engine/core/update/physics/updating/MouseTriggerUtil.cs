using Worms.engine.core.game_object_handler;
using Worms.engine.core.input;
using Worms.engine.data;
using Worms.engine.game_object.components.physics.colliders;

namespace Worms.engine.core.update.physics.updating; 

public static class MouseTriggerUtil {
    public static void UpdateMouseTriggers(TrackObject obj, bool doMouseClick) {
        bool isInsideTrigger = obj.Collider is { IsActive: true, state: ColliderState.TRIGGER }
                               && obj.Collider.IsPointInside(GetMouseWorldPosition(obj));

        switch (obj.MouseInsideTrigger) {
            case false when isInsideTrigger:
                PhysicsUtils.RunScriptsFunction(obj, static s => s.OnMouseEnter());
                break;
            case true when isInsideTrigger:
                PhysicsUtils.RunScriptsFunction(obj, static s => s.OnMouseOver());
                break;
            case true when !isInsideTrigger:
                PhysicsUtils.RunScriptsFunction(obj, static s => s.OnMouseExit());
                break;
        }

        if (doMouseClick && isInsideTrigger) {
            PhysicsUtils.RunScriptsFunction(obj, static s => s.OnMouseClick());
        } 
        
        obj.MouseInsideTrigger = isInsideTrigger;
    }
    
    private static Vector2 GetMouseWorldPosition(TrackObject trackObject) {
        return trackObject.isWorld ? Input.MouseWorldPosition : Input.MouseCameraPosition;
    }
}