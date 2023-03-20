using GameEngine.engine.core.game_object_handler;
using GameEngine.engine.core.input;
using GameEngine.engine.core.update.physics.layers;
using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components;
using GameEngine.engine.game_object.components.physics.colliders;

namespace GameEngine.engine.core.update.physics.updating; 

public static class TriggerResolver {
    public static void UpdateMouseTriggers(TrackObject obj, bool doMouseClick) {
        bool isInsideTrigger = obj.Colliders.Any(collider => 
            collider is { IsActive: true, state: ColliderState.TRIGGER }
                && collider.IsPointInside(GetMouseWorldPosition(obj))
        );

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
        foreach (Collider objCollider in obj.Colliders) {
            if (objCollider is not { IsActive: true, state: ColliderState.TRIGGER }) {
                return;
            }

            foreach ((GameObject gameObject, TrackObject checkObj) in objects) {
                if (!checkObj.isActive
                    || objCollider.gameObject == gameObject
                    || !LayerMask.CanLayersInteract(objCollider.gameObject.Layer, gameObject.Layer)
                   ) {
                    continue;
                }

                foreach (Collider checkObjCollider in checkObj.Colliders) {
                    if (checkObjCollider is not { IsActive: true }
                        || checkObjCollider.state != ColliderState.TRIGGERING_COLLIDER
                    ) {
                        continue;
                    }
                    
                    if (IntersectUtils.DoTriggersIntersect(objCollider, checkObjCollider)) {
                        collidersInTrigger.Add(checkObjCollider);
                    }
                }
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