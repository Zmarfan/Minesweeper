using System.Text;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class GameObject : Object {
    public delegate void GameObjectUpdate();
    public static event GameObjectUpdate? GameObjectUpdateEvent;
    
    public string Name { get; set; }

    public bool IsActive {
        get => _isActive;
        set {
            GameObjectUpdateEvent?.Invoke();;
            _isActive = value;
        }
    }
    private bool _isActive;

    public Transform Transform { get; }
    public readonly List<ToggleComponent> components;
    
    public GameObject(string name, bool isActive, Transform transform, List<ToggleComponent> components) {
        Name = name;
        IsActive = isActive;
        Transform = transform;
        this.components = components;
    }

    public GameObject GetRoot() {
        return Transform.Parent == null ? this : Transform.Parent.gameObject.GetRoot();
    }
    
    public GameObject GetParent() {
        return Transform.Parent!.gameObject;
    }
    
    public GameObjectBuilder AddSibling(string name) {
        return GameObjectBuilder.Builder(name, Transform.Parent?.gameObject!);
    }
    
    public GameObjectBuilder AddChild(string name) {
        return GameObjectBuilder.Builder(name, this);
    }

    public void AddChild(GameObjectBuilder builder) {
        builder.SetParent(this);
        builder.Build();
        GameObjectUpdateEvent?.Invoke();;
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