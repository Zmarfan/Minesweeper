using Worms.engine.data;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class GameObjectBuilder {
    private readonly string _name;
    private Transform? _parent;
    private Vector2 _localPosition = Vector2.Zero();
    private Rotation _localRotation = Rotation.Normal();
    private float _localScale = 1f;
    private bool _isActive = true;
    private readonly List<Component> _components = new();

    public GameObjectBuilder(string name, Transform? parent) {
        _name = name;
        _parent = parent;
    }

    public GameObject Build() {
        Transform transform = new(_parent, _localPosition, _localRotation, _localScale);
        GameObject gameObject = new(_name, _isActive, transform, _components);
        transform.InitComponent(gameObject);
        _components.ForEach(component => component.InitComponent(gameObject));
        return gameObject;
    }
    
    public static GameObjectBuilder Builder(string name) {
        return new GameObjectBuilder(name, null);
    }
    
    public static GameObjectBuilder Builder(string name, GameObject? parent) {
        return new GameObjectBuilder(name, parent?.Transform);
    }

    public GameObjectBuilder SetParent(GameObject parent) {
        _parent = parent.Transform;
        return this;
    }
    
    public GameObjectBuilder SetWorldPosition(Vector2 worldPosition) {
        Vector2 parentWorldPosition = _parent?.WorldPosition ?? Vector2.Zero();
        _localPosition = worldPosition - parentWorldPosition;
        return this;
    }
    
    public GameObjectBuilder SetLocalPosition(Vector2 localPosition) {
        _localPosition = localPosition;
        return this;
    }
    
    public GameObjectBuilder SetWorldRotation(Rotation worldRotation) {
        Rotation parentWorldRotation = _parent?.WorldRotation ?? Rotation.Normal();
        _localRotation = worldRotation - parentWorldRotation;
        return this;
    }
    
    public GameObjectBuilder SetLocalRotation(Rotation localRotation) {
        _localRotation = localRotation;
        return this;
    }

    public GameObjectBuilder SetWorldScale(float worldScale) {
        float parentWorldScale = _parent?.WorldScale ?? 1;
        _localScale = worldScale / parentWorldScale;
        return this;
    }
    
    public GameObjectBuilder SetLocalScale(float localScale) {
        _localScale = localScale;
        return this;
    }

    public GameObjectBuilder SetIsActive(bool isActive) {
        _isActive = isActive;
        return this;
    }

    public GameObjectBuilder SetComponent(Component component) {
        _components.Add(component);
        return this;
    }
}