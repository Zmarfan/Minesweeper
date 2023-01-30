using Worms.engine.core.game_object_handler;
using Worms.engine.core.input;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.colliders;
using Worms.engine.game_object.scripts;
using Worms.engine.logger;
using Worms.engine.scene;

namespace Worms.engine.core.update; 

public class PhysicsUpdateHandler {
    private readonly SceneData _sceneData;
    private GameObjectHandler GameObjectHandler => _sceneData.gameObjectHandler;

    public PhysicsUpdateHandler(SceneData sceneData) {
        _sceneData = sceneData;
    }

    public void Update() {
        foreach ((GameObject _, TrackObject obj) in GameObjectHandler.objects) {
            if (!obj.isActive) {
                return;
            }
            
            UpdateMouseTriggers(obj);
        }
    }

    private void UpdateMouseTriggers(TrackObject obj) {
        bool isInsideTrigger = obj.colliders
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