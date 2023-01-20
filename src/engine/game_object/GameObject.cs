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

    public Transform Transform { get; }
    public readonly List<ToggleComponent> components;
    
    public GameObject(string name, bool isActive, Transform transform, List<ToggleComponent> components) {
        Name = name;
        IsActive = isActive;
        Transform = transform;
        this.components = components;
    }

    public T GetComponent<T>() where T : ToggleComponent {
        return (T)components.First(static component => component is T);
    }
    
    public bool TryGetComponent<T>(out T component) where T : ToggleComponent {
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