using Worms.engine.core.game_object_handler;
using Worms.engine.core.input;
using Worms.engine.core.input.listener;
using Worms.engine.game_object;
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
            
            TriggerResolver.UpdateMouseTriggers(obj, _doMouseClick);
            TriggerResolver.UpdateColliderTriggers(obj, GameObjectHandler.objects);
        }
    }
}