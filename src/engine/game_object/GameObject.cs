using System.Text;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class GameObject : Object {
    public delegate void GameObjectUpdate(GameObject gameObject);
    public static event GameObjectUpdate? GameObjectActiveEvent;
    
    public string Name { get; set; }

    public bool IsActive {
        get => _isActive;
        set {
            bool changed = _isActive != value;
            _isActive = value;
            if (changed) {
                GameObjectActiveEvent?.Invoke(this);
            }
        }
    }
    private bool _isActive = true;

    public Transform Transform {
        get {
            if (_transform == null) {
                _transform = GetComponent<Transform>();
            }
            return _transform;
        }
    }
    public readonly List<Component> components;
    private Transform? _transform;
    
    public GameObject(string name, bool isActive, List<Component> components) {
        Name = name;
        IsActive = isActive;
        this.components = components;
    }

    public T GetComponent<T>() where T : Component {
        try {
            return (T)components.First(static component => component is T);
        }
        catch (InvalidOperationException e) {
            throw new Exception($"Unable to get component: {typeof(T)} from gameObject: {Name}");
        }
    }
    
    public bool TryGetComponent<T>(out T component) where T : Component {
        if (!components.Any(static component => component is T)) {
            component = default!;
            return false;
        }

        component = GetComponent<T>();
        return true;
    }

    public override string ToString() {
        string componentsString = components.Count == 0 ? string.Empty : components
            .Select(component => $"{component}\n")
            .Aggregate(new StringBuilder("Components: "), (acc, entry) => acc.Append(entry))
            .ToString();
        return $"GameObject: {Name}, IsActive: {IsActive}, Components: {componentsString}";
    }
}