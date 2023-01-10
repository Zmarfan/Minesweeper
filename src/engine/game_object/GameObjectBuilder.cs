using Worms.engine.data;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class GameObjectBuilder {
    private readonly string _name;
    private Transform? _parent;
    private Vector2 _localPosition = Vector2.Zero();
    private Rotation _localRotation = Rotation.Identity();
    private float _localScale = 1f;
    private bool _isActive = true;
    private readonly List<ToggleComponent> _components = new();

    private GameObjectBuilder(string name, Transform? parent) {
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

    public static GameObject Root() {
        return new GameObjectBuilder("root", null).Build();
    }

    public static GameObjectBuilder Builder(string name, GameObject parent) {
        return new GameObjectBuilder(name, parent.Transform);
    }

    public void SetParent(GameObject parent) {
        _parent = parent.Transform;
    }
    
    public GameObjectBuilder SetLocalPosition(Vector2 localPosition) {
        _localPosition = localPosition;
        return this;
    }

    public GameObjectBuilder SetLocalRotation(Rotation localRotation) {
        _localRotation = localRotation;
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

    public GameObjectBuilder SetComponent(ToggleComponent component) {
        _components.Add(component);
        return this;
    }
}