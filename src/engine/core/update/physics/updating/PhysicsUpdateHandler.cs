using Worms.engine.core.game_object_handler;
using Worms.engine.core.input;
using Worms.engine.core.input.listener;
using Worms.engine.core.update.physics.layers;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.scripts;
using Worms.engine.scene;

namespace Worms.engine.core.update.physics.updating; 

public class PhysicsUpdateHandler {
    private readonly SceneData _sceneData;
    private GameObjectHandler GameObjectHandler => _sceneData.gameObjectHandler;
    private bool _mouseIsDown;
    private bool _doMouseClick;

    public PhysicsUpdateHandler(SceneData sceneData) {
        _sceneData = sceneData;
        Physics.Init(sceneData);
    }

    public void Update() {
        bool down = Input.GetKey(Button.LEFT_MOUSE);
        _doMouseClick = !_mouseIsDown && down;
        _mouseIsDown = down;
        
        foreach ((GameObject _, TrackObject obj) in GameObjectHandler.objects) {
            if (!obj.isActive) {
                return;
            }
            
            MouseTriggerUtil.UpdateMouseTriggers(obj, _doMouseClick);
            TriggerIntersectUtils.UpdateColliderTriggers(obj, GameObjectHandler.objects);
        }
    }
}