using SDL2;
using Worms.engine.core.input.listener;
using Worms.engine.data;
using EventHandler = Worms.engine.core.event_handler.EventHandler;

namespace Worms.engine.core.input; 

public class Input {
    private static Input _self = null!;
    
    private readonly Dictionary<string, InputListener> _listenersByName;
    private readonly Dictionary<SDL.SDL_Scancode, InputListener> _listenersByScanCode = new();

    private Input(EventHandler eventHandler, List<InputListener> listeners) {
        _listenersByName = listeners.ToDictionary(l => l.name, l => l);
        listeners.ForEach(listener => {
            HashSet<SDL.SDL_Scancode> scanCodes = new();
            scanCodes.UnionWith(listener.negativeButtons);
            scanCodes.UnionWith(listener.positiveButtons);
            scanCodes.UnionWith(listener.altNegativeButtons);
            scanCodes.UnionWith(listener.altPositiveButtons);
            foreach (SDL.SDL_Scancode code in scanCodes) {
                _listenersByScanCode.Add(code, listener);
            }
        });

        eventHandler.KeyDownEvent += ButtonDownEventListener;
        eventHandler.KeyUpEvent += ButtonUpEventListener;
    }

    public static void Init(EventHandler eventHandler, List<InputListener> listeners) {
        if (_self != null) {
            throw new Exception("There can only be one input manager!");
        }

        _self = new Input(eventHandler, listeners);
    }

    public static void Update(float deltaTime) {
        foreach ((string _, InputListener listener) in _self._listenersByName) {
            listener.UpdateAxis(deltaTime);
        }
    }

    public static void FrameReset() {
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

    private void ButtonDownEventListener(SDL.SDL_Scancode scanCode) {
        if (_listenersByScanCode.ContainsKey(scanCode)) {
            _listenersByScanCode[scanCode].SetButtonDown(scanCode);
        }
    }
    
    private void ButtonUpEventListener(SDL.SDL_Scancode scanCode) {
        if (_listenersByScanCode.ContainsKey(scanCode)) {
            _listenersByScanCode[scanCode].SetButtonUp(scanCode);
        }
    }
}