using Worms.engine.game_object;
using Worms.engine.game_object.components;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;
using Object = Worms.engine.game_object.Object;

namespace Worms.engine.core; 

public class GameObjectHandler {
    public List<Script> AllScripts { get; private set; } = new();
    public List<Script> AllActiveGameObjectScripts { get; private set; } = new();
    public List<TextureRenderer> AllActiveGameObjectTextureRenderers { get; private set; } = new();
    private List<GameObject> _allGameObjects = new();

    private readonly Queue<Object> _destroyObjects = new();

    private readonly GameObject _root;

    public GameObjectHandler(GameObject root) {
        _root = root;
        OnGameObjectChange();
        GameObject.GameObjectActiveEvent += OnGameObjectChange;
        Transform.TransformHierarchyEvent += OnGameObjectChange;
        Object.ObjectDestroyEvent += OnObjectDestroy;
    }

    public void DestroyObjects() {
        bool didUpdate = _destroyObjects.Count > 0;
        while (_destroyObjects.Count > 0) {
            Object obj = _destroyObjects.Dequeue();
            if (obj is GameObject gameObject) {
                gameObject.Transform.Parent!.children.Remove(gameObject.Transform);
            }
            else if (obj is ToggleComponent component) {
                component.gameObject.components.Remove(component);
            }
        }

        if (didUpdate) {
            OnGameObjectChange();
        }
    }

    private void OnGameObjectChange() {
        List<ToggleComponent> allActive = GetAllComponents(true).ToList();

        _allGameObjects = GetAllGameObjectsFromGameObject(_root, true).ToList();
        AllActiveGameObjectTextureRenderers = allActive.OfType<TextureRenderer>().ToList();
        AllScripts = _allGameObjects.SelectMany(gameObject => gameObject.components).OfType<Script>().ToList();
        AllActiveGameObjectScripts = allActive.OfType<Script>().ToList();
    }

    private void OnObjectDestroy(Object obj) {
        _destroyObjects.Enqueue(obj);
    } 

    private IEnumerable<ToggleComponent> GetAllComponents(bool active) {
        return GetAllGameObjectsFromGameObject(_root, active).SelectMany(gameObject => gameObject.components);
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
        return gameObject.Transform.children.Select(static child => child.gameObject);
    }
}