using Worms.engine.game_object;
using Worms.engine.game_object.scripts;

namespace Worms.engine.core; 

public class GameObjectHandler {
    public List<GameObject> AllActiveGameObjects { get; private set; } = new();
    public List<GameObject> AllGameObjects { get; private set; } = new();

    public IEnumerable<Script> AwakeScripts => AllGameObjects.SelectMany(static gameObject => gameObject.components).OfType<Script>().Where(ShouldRunAwake);
    public IEnumerable<Script> StartScripts => AllActiveGameObjects.SelectMany(static gameObject => gameObject.components).OfType<Script>().Where(ShouldRunStart);
    public IEnumerable<Script> UpdateScripts => AllActiveGameObjects.SelectMany(static gameObject => gameObject.components).OfType<Script>().Where(ShouldRunUpdate);

    private readonly HashSet<Script> _hasRunAwake = new();
    private readonly HashSet<Script> _hasRunStart = new();
    
    private readonly GameObject _root;

    public GameObjectHandler(GameObject root) {
        _root = root;
        OnGameObjectChange();
        GameObject.GameObjectUpdateEvent += OnGameObjectChange;
    }

    ~GameObjectHandler() {
        GameObject.GameObjectUpdateEvent -= OnGameObjectChange;
    }

    public void MadeUpdateCycle() {
        foreach(Script script in AwakeScripts) {
            _hasRunAwake.Add(script);
        }
        foreach(Script script in StartScripts) {
            _hasRunStart.Add(script);
        }
    }
    
    private void OnGameObjectChange() {
        AllActiveGameObjects = GetAllGameObjectsFromGameObject(_root, true).ToList();
        AllGameObjects = GetAllGameObjectsFromGameObject(_root, false).ToList();
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
    
    private bool ShouldRunAwake(Script script) {
        return !_hasRunAwake.Contains(script);
    }
    
    private bool ShouldRunStart(Script script) {
        return script.IsActive && !_hasRunStart.Contains(script);
    }
    
    private static bool ShouldRunUpdate(Script script) {
        return script.IsActive;
    }
}