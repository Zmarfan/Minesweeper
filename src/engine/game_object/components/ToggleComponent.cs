namespace Worms.engine.game_object.components; 

public abstract class ToggleComponent : Component {
    public delegate void ToggleComponentActivityDelegate();

    public bool IsActive { get; set; }

    protected ToggleComponent(bool isActive) {
        IsActive = isActive;
    }
}