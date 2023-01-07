using System.Text;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class GameObject {
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public Transform Transform { get; }
    private List<Component> _components;
    
    public GameObject(string name, bool isActive, Transform transform, List<Component> components) {
        Name = name;
        IsActive = isActive;
        Transform = transform;
        _components = components;
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

    public override string ToString() {
        string components = _components.Count == 0 ? string.Empty : _components
            .Select(component => $"{component}\n")
            .Aggregate(new StringBuilder("Components: "), (acc, entry) => acc.Append(entry))
            .ToString();
        return $"GameObject: {Name}, IsActive: {IsActive}\nTransform: {Transform}\n{components}";
    }
}