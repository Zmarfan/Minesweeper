using SDL2;

namespace Worms.engine.core.input.listener; 

public class InputListenerBuilder {
    private readonly string _name;
    private HashSet<SDL.SDL_Scancode> _negativeButtons = new();
    private readonly HashSet<SDL.SDL_Scancode> _positiveButtons;
    private HashSet<SDL.SDL_Scancode> _altNegativeButtons = new();
    private HashSet<SDL.SDL_Scancode> _altPositiveButtons = new();
    private float _gravity = 3;
    private float _sensitivity = 3;
    private bool _snap = true;
    private InputAxis _axis = InputAxis.X_AXIS;

    private InputListenerBuilder(string name, HashSet<SDL.SDL_Scancode> positiveButtons) {
        _name = name;
        _positiveButtons = positiveButtons;
    }

    public static InputListenerBuilder Builder(string name, Button positiveButton) {
        return new InputListenerBuilder(name, ScanCodeToButton.BUTTON_TO_SCANCODE[positiveButton]);
    }

    public InputListener Build() {
        return new InputListener(
            _name, 
            _negativeButtons, 
            _positiveButtons, 
            _altNegativeButtons, 
            _altPositiveButtons, 
            _gravity,
            _sensitivity,
            _snap, 
            _axis
        );
    }

    public InputListenerBuilder SetNegativeButton(Button negativeButton) {
        _negativeButtons = ScanCodeToButton.BUTTON_TO_SCANCODE[negativeButton];
        return this;
    }
    
    public InputListenerBuilder SetAltNegativeButton(Button altNegativeButton) {
        _altNegativeButtons = ScanCodeToButton.BUTTON_TO_SCANCODE[altNegativeButton];
        return this;
    }
    
    public InputListenerBuilder SetAltPositiveButton(Button altPositiveButton) {
        _altPositiveButtons = ScanCodeToButton.BUTTON_TO_SCANCODE[altPositiveButton];
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