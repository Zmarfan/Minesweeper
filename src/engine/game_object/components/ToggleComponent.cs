namespace Worms.engine.game_object.components; 

public abstract class ToggleComponent : Component {
    public bool IsActive { get; set; }
    public string Name { get; set; }

    protected ToggleComponent(bool isActive = true, string name = "component") {
        IsActive = isActive;
        Name = name;
    }
    
    public virtual void OnDrawGizmos() {
    }
}