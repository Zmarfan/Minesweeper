using SDL2;

namespace Worms.engine.core.input.listener; 

public class InputListenerBuilder {
    private readonly string _name;
    private Button? _negativeButton;
    private readonly Button _positiveButton;
    private Button? _altNegativeButton;
    private Button? _altPositiveButton;
    private float _gravity = 3;
    private float _sensitivity = 3;
    private bool _snap = true;
    private InputAxis _axis = InputAxis.X_AXIS;

    private InputListenerBuilder(string name, Button positiveButton) {
        _name = name;
        _positiveButton = positiveButton;
    }

    public static InputListenerBuilder Builder(string name, Button positiveButton) {
        return new InputListenerBuilder(name, positiveButton);
    }

    public InputListener Build() {
        return new InputListener(
            _name, 
            _negativeButton, 
            _positiveButton, 
            _altNegativeButton, 
            _altPositiveButton, 
            _gravity,
            _sensitivity,
            _snap, 
            _axis
        );
    }

    public InputListenerBuilder SetNegativeButton(Button negativeButton) {
        _negativeButton = negativeButton;
        return this;
    }
    
    public InputListenerBuilder SetAltNegativeButton(Button altNegativeButton) {
        _altNegativeButton = altNegativeButton;
        return this;
    }
    
    public InputListenerBuilder SetAltPositiveButton(Button altPositiveButton) {
        _altPositiveButton = altPositiveButton;
        return this;
    }

    public InputListenerBuilder SetGravity(float gravity) {
        _gravity = gravity;
        return this;
    }
    
    public InputListenerBuilder SetSensitivity(float sensitivity) {
        _sensitivity = sensitivity;
        return this;
    }
    
    public InputListenerBuilder SetSnap(bool snap) {
        _snap = snap;
        return this;
    }
    
    public InputListenerBuilder SetAxis(InputAxis axis) {
        _axis = axis;
        return this;
    }
}