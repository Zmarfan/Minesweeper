using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.input.listener; 

public class InputListener {
    public readonly string name;
    public readonly HashSet<SDL.SDL_Scancode> negativeButtons;
    public readonly HashSet<SDL.SDL_Scancode> positiveButtons;
    public readonly HashSet<SDL.SDL_Scancode> altNegativeButtons;
    public readonly HashSet<SDL.SDL_Scancode> altPositiveButtons;
    public readonly float gravity;
    public readonly float sensitivity;
    public readonly bool snap;
    public readonly InputAxis axis;

    private float _value = 0;
    private bool _positiveDown = false;
    private bool _negativeDown = false;
    private bool _wasPositiveDown = false;

    public InputListener(
        string name,
        HashSet<SDL.SDL_Scancode> negativeButtons,
        HashSet<SDL.SDL_Scancode> positiveButtons,
        HashSet<SDL.SDL_Scancode> altNegativeButtons,
        HashSet<SDL.SDL_Scancode> altPositiveButtons,
        float gravity,
        float sensitivity,
        bool snap,
        InputAxis axis
    ) {
        this.name = name;
        this.negativeButtons = negativeButtons;
        this.positiveButtons = positiveButtons;
        this.altNegativeButtons = altNegativeButtons;
        this.altPositiveButtons = altPositiveButtons;
        this.gravity = gravity;
        this.sensitivity = sensitivity;
        this.snap = snap;
        this.axis = axis;
    }

    public bool GetButtonDown() {
        return _positiveDown && !_wasPositiveDown;
    }
    
    public bool GetButtonUp() {
        return !_positiveDown && _wasPositiveDown;
    }
    
    public bool GetButton() {
        return _positiveDown;
    }

    public Vector2 GetAxis() {
        return axis == InputAxis.X_AXIS ? new Vector2(_value, 0) : new Vector2(0, _value);
    }
    
    public void UpdateAxis(float deltaTime) {
        if ((!_negativeDown && !_positiveDown) || (_negativeDown && _positiveDown)) {
            if (_value > 0) {
                _value = Math.Max(_value - deltaTime * gravity, 0);
            }
            else {
                _value = Math.Min(_value + deltaTime * gravity, 0);
            }
            return;
        }
        if (_negativeDown) {
            if (_value > 0 && snap) {
                _value = 0;
            }

            _value -= deltaTime * sensitivity;
        }
        if (_positiveDown) {
            if (_value < 0 && snap) {
                _value = 0;
            }

            _value += deltaTime * sensitivity;
        }
        
        _value = Math.Clamp(_value, -1, 1);
    }
    
    public void SetButtonDown(SDL.SDL_Scancode scanCode) {
        _positiveDown = _positiveDown || positiveButtons.Contains(scanCode) || altPositiveButtons.Contains(scanCode);
        _negativeDown = _negativeDown || negativeButtons.Contains(scanCode) || altNegativeButtons.Contains(scanCode);
    }
    
    public void SetButtonUp(SDL.SDL_Scancode scanCode) {
        _positiveDown = _positiveDown && !(positiveButtons.Contains(scanCode) || altPositiveButtons.Contains(scanCode));
        _negativeDown = _negativeDown && !(negativeButtons.Contains(scanCode) || altNegativeButtons.Contains(scanCode));
    }


    public void FrameReset() {
        _wasPositiveDown = _positiveDown;
    }
}