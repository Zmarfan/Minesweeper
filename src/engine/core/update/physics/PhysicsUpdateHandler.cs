using Worms.engine.core.game_object_handler;
using Worms.engine.core.input;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.scripts;
using Worms.engine.scene;

namespace Worms.engine.core.update.physics; 

public class PhysicsUpdateHandler {
    private readonly SceneData _sceneData;
    private GameObjectHandler GameObjectHandler => _sceneData.gameObjectHandler;

    public PhysicsUpdateHandler(SceneData sceneData) {
        _sceneData = sceneData;
        Physics.Init(sceneData);
    }

    public void Update() {
        foreach ((GameObject _, TrackObject obj) in GameObjectHandler.objects) {
            if (!obj.isActive) {
                return;
            }
            
            UpdateMouseTriggers(obj);
            UpdateColliderTriggers(obj);
        }
    }

    private void UpdateMouseTriggers(TrackObject obj) {
        bool isInsideTrigger = obj.Colliders
            .Where(collider => collider is { IsActive: true, isTrigger: true })
            .Any(trigger => trigger.IsPointInside(GetMouseWorldPosition(obj)));
        
        if (!obj.MouseInsideTrigger && isInsideTrigger) {
            RunScriptsFunction(obj, static s => s.OnMouseEnter());
        }
        else if (obj.MouseInsideTrigger && isInsideTrigger) {
            RunScriptsFunction(obj, static s => s.OnMouseOver());
        }
        else if (obj.MouseInsideTrigger && !isInsideTrigger) {
            RunScriptsFunction(obj, static s => s.OnMouseExit());
        }

        obj.MouseInsideTrigger = isInsideTrigger;
    }
    
    private void UpdateColliderTriggers(TrackObject obj) {
        HashSet<Collider> collidersInTrigger = new();
        foreach (Collider collider in obj.Colliders) {
            if (collider is not { IsActive: true, isTrigger: true }) {
                continue;
            }

            foreach ((GameObject _, TrackObject checkObj) in GameObjectHandler.objects) {
                if (!obj.isActive || (obj.RigidBody == null && checkObj.RigidBody == null)) {
                    continue;
                }

                foreach (Collider checkCollider in checkObj.Colliders) {
                    if (checkCollider is not { IsActive: true, isTrigger: false }) {
                        continue;
                    }

                    if (DoCollidersIntersect(collider, checkCollider)) {
                        collidersInTrigger.Add(checkCollider);
                    }
                }
            }
        }

        FireObjectTriggerEvents(obj, collidersInTrigger);
        obj.CollidersInsideTrigger = collidersInTrigger;
    }

    private static bool DoCollidersIntersect(Collider c1, Collider c2) {
        return c1 switch {
            BoxCollider box1 when c2 is BoxCollider box2 => TriggerIntersectUtils.DoesBoxOnBoxOverlap(box1, box2),
            CircleCollider circle1 when c2 is CircleCollider circle2 => TriggerIntersectUtils.DoesCircleOnCircleOverlap(circle1, circle2),
            PixelCollider when c2 is PixelCollider => TriggerIntersectUtils.DoesPixelOnPixelOverlap(c1, c2),
            BoxCollider when c2 is CircleCollider => TriggerIntersectUtils.DoesBoxOnCircleOverlap(c1, c2),
            BoxCollider when c2 is PixelCollider => TriggerIntersectUtils.DoesBoxOnPixelOverlap(c1, c2),
            CircleCollider when c2 is PixelCollider => TriggerIntersectUtils.DoesCircleOnPixelOverlap(c1, c2),
            PixelCollider when c2 is CircleCollider => TriggerIntersectUtils.DoesCircleOnPixelOverlap(c2, c1),
            CircleCollider when c2 is BoxCollider => TriggerIntersectUtils.DoesBoxOnCircleOverlap(c2, c1),
            PixelCollider when c2 is BoxCollider => TriggerIntersectUtils.DoesBoxOnPixelOverlap(c2, c1),
            _ => throw new Exception($"The collider types: {c1} and {c2} are not supported in the physics trigger system!")
        };
    }

    private static void FireObjectTriggerEvents(TrackObject obj, IReadOnlySet<Collider> collidersInTrigger) {
        foreach (Collider collider in obj.CollidersInsideTrigger) {
            if (collidersInTrigger.Contains(collider)) {
                RunScriptsFunction(obj, script => script.OnTriggerStay(collider));
            }
            else {
                RunScriptsFunction(obj, script => script.OnTriggerExit(collider));
            }
        }

        foreach (Collider collider in collidersInTrigger.Except(obj.CollidersInsideTrigger)) {
            RunScriptsFunction(obj, script => script.OnTriggerEnter(collider));
        }
    }
    
    private static Vector2 GetMouseWorldPosition(TrackObject trackObject) {
        return trackObject.isWorld ? Input.MouseWorldPosition : Input.MouseCameraPosition;
    }

    private static void RunScriptsFunction(TrackObject obj, Action<Script> action) {
        foreach (Script script in obj.Scripts) {
            if (script.IsActive) {
                action.Invoke(script);
            }
        }
    }
}