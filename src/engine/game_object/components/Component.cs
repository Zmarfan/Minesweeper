namespace Worms.engine.game_object.components; 

public abstract class Component : Object {
    public GameObject gameObject = null!;
    public Transform Transform => gameObject.Transform;

    public void InitComponent(GameObject gameObj) {
        gameObject = gameObj;
    }
    
    public T GetComponent<T>() where T : Component {
        return gameObject.GetComponent<T>();
    }
    
    public bool TryGetComponent<T>(out T component) where T : Component {
        return gameObject.TryGetComponent(out component);
    }
}