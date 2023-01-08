namespace Worms.engine.game_object.components; 

public abstract class ToggleComponent : Component {
    public delegate void ToggleComponentActivityDelegate();
    public static event ToggleComponentActivityDelegate? ActivityUpdateEvent;

    public bool IsActive {
        get => _isActive;
        set {
            ActivityUpdateEvent?.Invoke();
            _isActive = value;
        }
    }
    private bool _isActive;

    protected ToggleComponent(bool isActive) {
        IsActive = isActive;
    }
}