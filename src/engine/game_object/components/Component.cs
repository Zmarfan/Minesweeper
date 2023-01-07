namespace Worms.engine.game_object.components; 

public abstract class Component {
    public GameObject GameObject { get; private set; } = null!;
    public Transform Transform => GameObject.Transform;

    public void InitComponent(GameObject gameObject) {
        GameObject = gameObject;
    }
}