using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.input.listener; 

public class InputListener {
    public readonly string name;
    public readonly Button? negativeButton;
    public readonly Button positiveButton;
    public readonly Button? altNegativeButton;
    public readonly Button? altPositiveButton;
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
        Button? negativeButton,
        Button positiveButton,
        Button? altNegativeButton,
        Button? altPositiveButton,
        float gravity,
        float sensitivity,
        bool snap,
        InputAxis axis
    ) {
        this.name = name;
        this.negativeButton = negativeButton;
        this.positiveButton = positiveButton;
        this.altNegativeButton = altNegativeButton;
        this.altPositiveButton = altPositiveButton;
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
        if (ShouldPerformGravityOperation()) {
            if (_value > 0) {
                _value = Math.Max(_value - deltaTime * gravity, 0);
            }
            else {
                _value = Math.Min(_value + deltaTime * gravity, 0);
            }
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

    public void SetButtonDown(Button button) {
        _positiveDown = _positiveDown || positiveButton == button || altPositiveButton == button;
        _negativeDown = _negativeDown || negativeButton == button || altNegativeButton == button;
    }
    
    public void SetButtonUp(Button button) {
        _positiveDown = _positiveDown && !(positiveButton == button || altPositiveButton == button);
        _negativeDown = _negativeDown && !(negativeButton == button || altNegativeButton == button);
    }


    public void FrameReset() {
        _wasPositiveDown = _positiveDown;
    }
    
    private bool ShouldPerformGravityOperation() {
        return (!_negativeDown && !_positiveDown)
               || (_negativeDown && _positiveDown)
               || (_positiveDown && _value < 0 && !snap)
               || (_negativeDown && _value > 0 && !snap);
    }
}