using Worms.engine.core.game_object_handler;
using Worms.engine.core.input;
using Worms.engine.core.update.physics.layers;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components;
using Worms.engine.game_object.components.physics.colliders;

namespace Worms.engine.core.update.physics.updating; 

public static class TriggerResolver {
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

    public static void UpdateColliderTriggers(TrackObject obj, Dictionary<GameObject, TrackObject> objects) {
        HashSet<Collider> collidersInTrigger = new();
        if (obj.Collider is not { IsActive: true, state: ColliderState.TRIGGER }) {
            return;
        }

        foreach ((GameObject gameObject, TrackObject checkObj) in objects) {
            if (!checkObj.isActive
                || obj.Collider.gameObject == gameObject
                || !LayerMask.CanLayersInteract(obj.Collider.gameObject.Layer, gameObject.Layer)
                || (obj.RigidBody == null && checkObj.RigidBody == null)
                || checkObj.Collider is not { IsActive: true }
                || checkObj.Collider.state != ColliderState.TRIGGERING_COLLIDER
               ) {
                continue;
            }

            if (IntersectUtils.DoTriggersIntersect(obj.Collider, checkObj.Collider)) {
                collidersInTrigger.Add(checkObj.Collider);
            }
        }

        FireObjectTriggerEvents(obj, collidersInTrigger);
        obj.CollidersInsideTrigger = collidersInTrigger;
    }

    private static Vector2 GetMouseWorldPosition(TrackObject trackObject) {
        return trackObject.isWorld ? Input.MouseWorldPosition : Input.MouseCameraPosition;
    }
    
    private static void FireObjectTriggerEvents(TrackObject obj, IReadOnlySet<Collider> collidersInTrigger) {
        foreach (Collider collider in obj.CollidersInsideTrigger) {
            if (collidersInTrigger.Contains(collider)) {
                PhysicsUtils.RunScriptsFunction(obj, script => script.OnTriggerStay(collider));
            }
            else {
                PhysicsUtils.RunScriptsFunction(obj, script => script.OnTriggerExit(collider));
            }
        }

        foreach (Collider collider in collidersInTrigger.Except(obj.CollidersInsideTrigger)) {
            PhysicsUtils.RunScriptsFunction(obj, script => script.OnTriggerEnter(collider));
        }
    }
}