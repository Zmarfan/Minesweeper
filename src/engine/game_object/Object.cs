namespace Worms.engine.game_object; 

public abstract class Object {
    public bool ShouldDestroy { get; private set; }

    public virtual void Destroy() {
        ShouldDestroy = true;
    }
}