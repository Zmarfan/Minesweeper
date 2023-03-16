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
    
    public List<T> GetComponents<T>() where T : Component {
        return gameObject.GetComponents<T>();
    }
    
    public bool TryGetComponent<T>(out T component) where T : Component {
        return gameObject.TryGetComponent(out component);
    }

    public T GetComponentInChildren<T>() where T : Component {
        return gameObject.GetComponentInChildren<T>();
    }
    
    public T AddComponent<T>(T component) where T : ToggleComponent {
        component.InitComponent(gameObject);
        return gameObject.AddComponent(component);
    }

    public virtual void OnDestroy() {
    }
}