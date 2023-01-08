namespace Worms.engine.game_object.components; 

public abstract class Component {
    public GameObject gameObject = null!;
    public Transform Transform => gameObject.Transform;

    public void InitComponent(GameObject gameObj) {
        gameObject = gameObj;
    }
    
    public T GetComponent<T>() where T : ToggleComponent {
        return gameObject.GetComponent<T>();
    }
    
    public bool TryGetComponent<T>(out T component) where T : ToggleComponent {
        return gameObject.TryGetComponent(out component);
    }
}