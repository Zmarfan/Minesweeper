using Worms.engine.data;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class GameObjectBuilder {
    private readonly string _name;
    private Transform? _parent;
    private Vector2 _position = Vector2.Zero();
    private Rotation _rotation = Rotation.Identity();
    private Vector2 _scale = Vector2.One();
    private bool _isActive = true;
    private readonly List<ToggleComponent> _components = new();

    private GameObjectBuilder(string name, Transform? parent) {
        _name = name;
        _parent = parent;
    }

    public GameObject Build() {
        Transform transform = new(_parent, _position, _rotation, _scale);
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

    public void SetParent(Transform parent) {
        _parent = parent;
    }
    
    public GameObjectBuilder SetPosition(Vector2 position) {
        _position = position;
        return this;
    }

    public GameObjectBuilder SetRotation(Rotation rotation) {
        _rotation = rotation;
        return this;
    }

    public GameObjectBuilder SetScale(Vector2 scale) {
        _scale = scale;
        return this;
    }
    
    public GameObjectBuilder SetScale(float scale) {
        _scale = new Vector2(scale, scale);
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