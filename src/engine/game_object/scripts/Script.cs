using Worms.engine.game_object.components;

namespace Worms.engine.game_object.scripts; 

public abstract class Script : ToggleComponent {
    public bool HasRunAwake { get; set; }
    public bool HasRunStart { get; set; }
    
    protected Script(bool isActive) : base(isActive) {
    }

    public virtual void Awake() {
    }

    public virtual void Start() {
    }

    public virtual void Update(float deltaTime) {
    }

    public virtual void OnDrawGizmos() {
    }
}