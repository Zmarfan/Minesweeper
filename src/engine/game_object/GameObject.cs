using System.Text;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class GameObject {
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public Transform Transform { get; }
    private readonly List<Component> _components;
    
    public GameObject(string name, bool isActive, Transform transform, List<Component> components) {
        Name = name;
        IsActive = isActive;
        Transform = transform;
        this._components = components;
    }

    public GameObject GetRoot() {
        return Transform.Parent == null ? this : Transform.Parent.GameObject.GetRoot();
    }
    
    public GameObjectBuilder AddSibling(string name) {
        return GameObjectBuilder.Builder(name, Transform.Parent?.GameObject);
    }
    
    public GameObjectBuilder AddChild(string name) {
        return GameObjectBuilder.Builder(name, this);
    }

    public void AddChild(GameObjectBuilder builder) {
        builder.SetParent(this);
        builder.Build();
    }

    public T GetComponent<T>() {
        return (T)Convert.ChangeType(_components.First(static component => component is T), typeof(T));
    }
    
    public bool TryGetComponent<T>(out T component) {
        if (!_components.Any(static component => component is T)) {
            component = default!;
            return false;
        }

        component = GetComponent<T>();
        return true;
    }

    public override string ToString() {
        string componentsString = this._components.Count == 0 ? string.Empty : this._components
            .Select(component => $"{component}\n")
            .Aggregate(new StringBuilder("Components: "), (acc, entry) => acc.Append(entry))
            .ToString();
        return $"GameObject: {Name}, IsActive: {IsActive}, Components: {componentsString}";
    }
}