using Worms.engine.game_object.components;

namespace Worms.engine.game_object.scripts; 

public abstract class Script : ToggleComponent {
    protected Script(bool isActive) : base(isActive) {
    }

    public virtual void Awake() {
    }

    public virtual void Start() {
    }

    public virtual void Update(float deltaTime) {
    }
}