using GameEngine.engine.core.game_object_handler;
using GameEngine.engine.core.input;
using GameEngine.engine.core.input.listener;
using GameEngine.engine.game_object;
using GameEngine.engine.scene;
using Button = GameEngine.engine.core.input.listener.Button;

namespace GameEngine.engine.core.update.physics.updating; 

internal class PhysicsUpdateHandler {
    private readonly SceneData _sceneData;
    private GameObjectHandler GameObjectHandler => _sceneData.gameObjectHandler;
    private readonly Dictionary<Button, MouseInputStatus> _mouseClickStatus = new() {
        { Button.LEFT_MOUSE, new MouseInputStatus() },
        { Button.RIGHT_MOUSE, new MouseInputStatus() },
        { Button.MIDDLE_MOUSE, new MouseInputStatus() },
    };


    public PhysicsUpdateHandler(SceneData sceneData) {
        _sceneData = sceneData;
        Physics.Init(sceneData);
    }

    public void Update() {
        HandleMouseInput(Button.LEFT_MOUSE);
        HandleMouseInput(Button.RIGHT_MOUSE);
        HandleMouseInput(Button.MIDDLE_MOUSE);
        MouseClickMask clickMask = CreateClickMask();
        
        foreach ((GameObject _, TrackObject obj) in GameObjectHandler.objects) {
            if (!obj.isActive) {
                return;
            }
            
            TriggerResolver.UpdateMouseTriggers(obj, clickMask);
            TriggerResolver.UpdateColliderTriggers(obj, GameObjectHandler.objects);
        }
    }

    private MouseClickMask CreateClickMask() {
        int clickMask = 0;
        
        if (_mouseClickStatus[Button.LEFT_MOUSE].clicked) {
            clickMask |= MouseClickMask.LEFT;
        }
        if (_mouseClickStatus[Button.RIGHT_MOUSE].clicked) {
            clickMask |= MouseClickMask.RIGHT;
        }
        if (_mouseClickStatus[Button.MIDDLE_MOUSE].clicked) {
            clickMask |= MouseClickMask.MIDDLE;
        }

        return new MouseClickMask(clickMask);
    }

    private void HandleMouseInput(Button mouseButton) {
        bool buttonDown = Input.GetKey(mouseButton);
        _mouseClickStatus[mouseButton].clicked = !_mouseClickStatus[mouseButton].isDown && buttonDown;
        _mouseClickStatus[mouseButton].isDown = buttonDown;
    }
}