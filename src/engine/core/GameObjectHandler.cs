using Worms.engine.game_object;
using Worms.engine.game_object.components.texture_renderer;

namespace Worms.engine.core; 

public class GameObjectHandler {
    public List<GameObject> AllActiveGameObjects { get; private set; } = new();
    public List<TextureRenderer> AllActiveTextureRenderers { get; private set; } = new();
    private readonly GameObject _root;
    
    public GameObjectHandler(GameObject root) {
        _root = root;
        OnGameObjectChange();
        GameObject.GameObjectUpdateEvent += OnGameObjectChange;
    }

    ~GameObjectHandler() {
        GameObject.GameObjectUpdateEvent -= OnGameObjectChange;
    }

    private void OnGameObjectChange() {
        AllActiveGameObjects = GetAllGameObjectsFromGameObject(_root, true).ToList();
        AllActiveTextureRenderers = GetAllComponentsOfTypeFromGameObject<TextureRenderer>().ToList();
    }
    
    private IEnumerable<T> GetAllComponentsOfTypeFromGameObject<T>() {
        List<T> components = new();
        AllActiveGameObjects
            .ToList()
            .ForEach(gameObject => {
                if (gameObject.TryGetComponent(out T component)) {
                    components.Add(component);
                }
            });
        return components;
    }
    
    private static IEnumerable<GameObject> GetAllGameObjectsFromGameObject(GameObject gameObject, bool active) {
        if (!gameObject.IsActive && active) {
            return new List<GameObject>();
        }
        List<GameObject> gameObjects = new() { gameObject };
        foreach (GameObject child in GetChildren(gameObject)) {
            gameObjects.AddRange(GetAllGameObjectsFromGameObject(child, active));
        }

        return gameObjects;
    }

    private static IEnumerable<GameObject> GetChildren(GameObject gameObject) {
        return gameObject.Transform.children.Select(static child => child.GameObject);
    }
}