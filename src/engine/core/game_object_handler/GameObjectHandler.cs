using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components;
using GameEngine.engine.game_object.scripts;
using GameEngine.engine.helper;
using GameEngine.engine.logger;
using Object = GameEngine.engine.game_object.Object;

namespace GameEngine.engine.core.game_object_handler; 

internal class GameObjectHandler {
    public readonly Dictionary<GameObject, TrackObject> objects = new();

    private readonly Queue<GameObject> _instantiatedGameObjects = new();
    private readonly HashSet<ToggleComponent> _addedGameObjectComponents = new();
    private readonly HashSet<Object> _destroyObjects = new();
    private readonly HashSet<GameObject> _activeStatusChangedGameObjects = new();

    private readonly GameObject _worldRoot;
    private readonly GameObject _screenRoot;

    public GameObjectHandler(GameObject worldRoot, GameObject screenRoot) {
        _worldRoot = worldRoot;
        _screenRoot = screenRoot;
        
        Transform.GameObjectInstantiateEvent += obj => _instantiatedGameObjects.Enqueue(obj);
        GameObject.GameObjectComponentAdd +=  component => _addedGameObjectComponents.Add(component);
        Object.ObjectDestroyEvent += obj => _destroyObjects.Add(obj);
        GameObject.GameObjectActiveEvent += obj => _activeStatusChangedGameObjects.Add(obj);
        
        _instantiatedGameObjects.Enqueue(worldRoot);
        _instantiatedGameObjects.Enqueue(screenRoot);
        FrameCleanup();
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

    public void DestroyScene() {
        DestroyGameObject(_worldRoot);
        DestroyGameObject(_screenRoot);
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
        if (objects.ContainsKey(gameObject)) {
            return;
        }
        
        List<GameObject> allGameObjects = GetAllGameObjectsFromGameObject(gameObject, false).ToList();
        HashSet<GameObject> allActiveGameObjects = GetAllGameObjectsFromGameObject(gameObject, true).ToHashSet();
        allGameObjects.ForEach(obj => {
            bool isWorld = obj.Transform.GetRoot() == _worldRoot.Transform;
            TrackObject trackObject = new(isWorld, allActiveGameObjects.Contains(obj), obj.components.OfType<ToggleComponent>().ToList());
            objects.Add(obj, trackObject);
        });
        
        foreach (Script script in allGameObjects.SelectMany(obj => objects[obj].Scripts)) {
            RunAwakeOnScript(script);
        }
    }

    private void InstantiateComponent(ToggleComponent component) {
        component.gameObject.components.Add(component);
        objects[component.gameObject].toggleComponents.Add(component);
        if (component is Script script) {
            RunAwakeOnScript(script);
        }
    }

    private static void RunAwakeOnScript(Script script) {
        try {
            if (!script.HasRunAwake) {
                script.Awake();
            }
        }
        catch (Exception e) {
            Logger.Error(e, $"An exception occured in {script} during the Awake callback");
        }
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
                if (gameObject.Transform != gameObject.Transform.GetRoot()) {
                    gameObject.Transform.Parent!.children.Remove(gameObject.Transform);
                }
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