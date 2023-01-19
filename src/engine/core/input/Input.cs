using SDL2;
using Worms.engine.camera;
using Worms.engine.core.input.listener;
using Worms.engine.data;
using EventHandler = Worms.engine.core.event_handler.EventHandler;

namespace Worms.engine.core.input; 

public class Input {
    private static Input _self = null!;

    public static Vector2 MouseWorldPosition =>
        _gameSettings.camera.ScreenToWorldMatrix.ConvertPoint(new Vector2(
            MouseScreenPosition.x * _gameSettings.width,
            (1 - MouseScreenPosition.y) * _gameSettings.height)
        );

    public static Vector2 MouseScreenPosition { get; private set; }
    public static Vector2 MouseDirection { get; private set; }
    private static GameSettings _gameSettings = null!;
    
    private readonly Dictionary<string, InputListener> _listenersByName;
    private readonly Dictionary<Button, InputListener> _listenersByButton = new();

    private Input(GameSettings settings, EventHandler eventHandler, List<InputListener> listeners) {
        _gameSettings = settings;
        
        _listenersByName = listeners.ToDictionary(l => l.name, l => l);
        listeners.ForEach(listener => {
            _listenersByButton.Add(listener.positiveButton, listener);
            if (listener.negativeButton != null) {
                _listenersByButton.Add((Button)listener.negativeButton, listener);
            }
            if (listener.altPositiveButton != null) {
                _listenersByButton.Add((Button)listener.altPositiveButton, listener);
            }
            if (listener.altNegativeButton != null) {
                _listenersByButton.Add((Button)listener.altNegativeButton, listener);
            }
        });

        eventHandler.KeyDownEvent += ButtonDownEventListener;
        eventHandler.KeyUpEvent += ButtonUpEventListener;
        eventHandler.MouseMovementEvent += (position, direction) => {
            MouseScreenPosition = position;
            MouseDirection = direction;
        };
    }

    public static void Init(GameSettings settings, EventHandler eventHandler, List<InputListener> listeners) {
        if (_self != null) {
            throw new Exception("There can only be one input manager!");
        }

        _self = new Input(settings, eventHandler, listeners);
    }

    public static void Update(float deltaTime) {
        foreach ((string _, InputListener listener) in _self._listenersByName) {
            listener.UpdateAxis(deltaTime);
        }
    }

    public static void FrameReset() {
        MouseDirection = Vector2.Zero();
        foreach ((string _, InputListener listener) in _self._listenersByName) {
            listener.FrameReset();
        }
    }

    public static bool GetButtonDown(string listenerName) {
        return _self._listenersByName[listenerName].GetButtonDown();
    }

    public static bool GetButtonUp(string listenerName) {
        return _self._listenersByName[listenerName].GetButtonUp();
    }
    
    public static bool GetButton(string listenerName) {
        return _self._listenersByName[listenerName].GetButton();
    }
    
    public static Vector2 GetAxis(string listenerName) {
        return _self._listenersByName[listenerName].GetAxis();
    }

    private void ButtonDownEventListener(Button button) {
        if (_listenersByButton.TryGetValue(button, out InputListener? value)) {
            value.SetButtonDown(button);
        }
    }
    
    private void ButtonUpEventListener(Button button) {
        if (_listenersByButton.TryGetValue(button, out InputListener? value)) {
            value.SetButtonUp(button);
        }
    }
}