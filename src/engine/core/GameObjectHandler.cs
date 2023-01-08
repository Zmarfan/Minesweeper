using Worms.engine.game_object;
using Worms.engine.game_object.components;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.engine.core; 

public class GameObjectHandler {
    public List<GameObject> AllActiveGameObjects { get; private set; } = new();
    public List<TextureRenderer> AllActiveTextureRenderers { get; private set; } = new();

    public List<Script> AwakeScripts { get; private set; } = new();
    public List<Script> StartScripts { get; private set; } = new();
    public List<Script> UpdateScripts { get; private set; } = new();
    private List<Script> _allScripts = new();
    private readonly HashSet<Script> _hasRunAwake = new();
    private readonly HashSet<Script> _hasRunStart = new();
    
    private readonly GameObject _root;

    public GameObjectHandler(GameObject root) {
        _root = root;
        OnGameObjectChange();
        OnToggleComponentChange();
        GameObject.GameObjectUpdateEvent += OnGameObjectChange;
        ToggleComponent.ActivityUpdateEvent += OnToggleComponentChange;
    }

    ~GameObjectHandler() {
        GameObject.GameObjectUpdateEvent -= OnGameObjectChange;
        ToggleComponent.ActivityUpdateEvent -= OnToggleComponentChange;
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
    }
    
    private void OnToggleComponentChange() {
        AllActiveTextureRenderers = GetAllComponentsOfTypeFromGameObject<TextureRenderer>(true).ToList();
        _allScripts = GetAllComponentsOfTypeFromGameObject<Script>(false).ToList();
        AwakeScripts = _allScripts.Where(ShouldRunAwake).ToList();
        StartScripts = _allScripts.Where(ShouldRunStart).ToList();
        UpdateScripts = _allScripts.Where(ShouldRunUpdate).ToList();
    }
    
    private IEnumerable<T> GetAllComponentsOfTypeFromGameObject<T>(bool active) where T : ToggleComponent {
        List<T> components = new();
        AllActiveGameObjects
            .ToList()
            .ForEach(gameObject => {
                if (!gameObject.TryGetComponent(out T component))
                    return;
                if (component is { IsActive: true } || !active) {
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