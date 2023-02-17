using System.Text;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class GameObject : Object {
    public delegate void GameObjectUpdate(GameObject gameObject);
    public delegate void ComponentAdd(ToggleComponent component);
    public static event GameObjectUpdate? GameObjectActiveEvent;
    public static event ComponentAdd? GameObjectComponentAdd;
    
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

    public Transform Transform => _transform ??= GetComponent<Transform>();
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
        catch (InvalidOperationException) {
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

    public T AddComponent<T>(T component) where T : ToggleComponent {
        component.InitComponent(this);
        GameObjectComponentAdd?.Invoke(component);
        return component;
    }
    
    public override string ToString() {
        string componentsString = components.Count == 0 ? string.Empty : components
            .Select(component => $"{component}\n")
            .Aggregate(new StringBuilder("Components: "), (acc, entry) => acc.Append(entry))
            .ToString();
        return $"GameObject: {Name}, IsActive: {IsActive}, Components: {componentsString}";
    }
}