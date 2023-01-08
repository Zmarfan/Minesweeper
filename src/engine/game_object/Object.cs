namespace Worms.engine.game_object; 

public abstract class Object {
    public delegate void ObjectDestroyDelegate(Object obj);

    public static event ObjectDestroyDelegate? ObjectDestroyEvent;

    public void Destroy() {
        ObjectDestroyEvent?.Invoke(this);
    }
}