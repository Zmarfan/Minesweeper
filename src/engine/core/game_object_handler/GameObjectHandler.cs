using Worms.engine.game_object;
using Worms.engine.game_object.components;
using Worms.engine.helper;
using Object = Worms.engine.game_object.Object;

namespace Worms.engine.core.game_object_handler; 

public class GameObjectHandler {
    public readonly Dictionary<GameObject, TrackObject> objects = new();

    private readonly Queue<GameObject> _instantiatedGameObjects = new();
    private readonly HashSet<ToggleComponent> _addedGameObjectComponents = new();
    private readonly HashSet<Object> _destroyObjects = new();
    private readonly HashSet<GameObject> _activeStatusChangedGameObjects = new();

    private readonly GameObject _worldRoot;

    public GameObjectHandler(GameObject worldRoot, GameObject screenRoot) {
        _worldRoot = worldRoot;
        
        InstantiateGameObject(worldRoot);
        InstantiateGameObject(screenRoot);
        Transform.GameObjectInstantiateEvent += obj => _instantiatedGameObjects.Enqueue(obj);
        GameObject.GameObjectComponentAdd +=  component => _addedGameObjectComponents.Add(component);
        Object.ObjectDestroyEvent += obj => _destroyObjects.Add(obj);
        GameObject.GameObjectActiveEvent += obj => _activeStatusChangedGameObjects.Add(obj);
    }

    public void FrameCleanup() {
        while (_instantiatedGameObjects.Count > 0) {
            InstantiateGameObject(_instantiatedGameObjects.Dequeue());
        }
        while (_addedGameObjectComponents.Count > 0) {
            ToggleComponent component = _addedGameObjectComponents.First();
            InstantiateComponent(component);
            _addedGameObjectComponents.Remove(component);
        }
        while (_destroyObjects.Count > 0) {
            Object obj = _destroyObjects.First();
            DestroyObject(obj);
            _destroyObjects.Remove(obj);
        }
        while (_activeStatusChangedGameObjects.Count > 0) {
            GameObject obj = _activeStatusChangedGameObjects.First();
            ChangeActiveGameObject(obj);
            _activeStatusChangedGameObjects.Remove(obj);
        }
    }
    
    private void DestroyObject(Object obj) {
        switch (obj) {
            case GameObject gameObject:
                DestroyGameObject(gameObject);
                break;
            case ToggleComponent component:
                DestroyComponent(component);
                break;
        }
    }

    private void InstantiateGameObject(GameObject gameObject) {
        List<GameObject> allGameObjects = GetAllGameObjectsFromGameObject(gameObject, false).ToList();
        HashSet<GameObject> allActiveGameObjects = GetAllGameObjectsFromGameObject(gameObject, true).ToHashSet();
        allGameObjects.ForEach(obj => {
            bool isWorld = obj.Transform.GetRoot() == _worldRoot.Transform;
            TrackObject trackObject = new(isWorld, allActiveGameObjects.Contains(obj), obj.components.OfType<ToggleComponent>().ToList());
            objects.Add(obj, trackObject);
        });
    }

    private void InstantiateComponent(ToggleComponent component) {
        component.gameObject.components.Add(component);
        objects[component.gameObject].toggleComponents.Add(component);
    }

    private void ChangeActiveGameObject(GameObject gameObject) {
        ChangeActiveGameObject(gameObject, gameObject.IsActive);
    }

    private void ChangeActiveGameObject(GameObject gameObject, bool active) {
        GetAllGameObjectsFromGameObject(gameObject, false)
            .ToList()
            .ForEach(obj => {
                objects[obj].isActive = active;
            });
    }

    private void DestroyGameObject(GameObject gameObject) {
        GetAllGameObjectsFromGameObject(gameObject, false)
            .ToList()
            .ForEach(obj => {
                obj.components.ForEach(component => component.OnDestroy());
                gameObject.Transform.Parent!.children.Remove(gameObject.Transform);
                objects.Remove(obj);
            });
    }
    
    private void DestroyComponent(ToggleComponent component) {
        component.OnDestroy();
        component.gameObject.components.Remove(component);
        objects[component.gameObject].RemoveComponent(component);
    }

    private static IEnumerable<GameObject> GetAllGameObjectsFromGameObject(GameObject gameObject, bool active) {
        if (!gameObject.IsActive && active) {
            return ListUtils.Empty<GameObject>();
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