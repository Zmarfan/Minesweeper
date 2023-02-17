namespace Worms.engine.scene; 

public class SceneManager {
    private static SceneManager _self = null!;

    public static string ActiveScene { get; private set; } = null!;
    private readonly Dictionary<string, Scene> _scenes;
    private readonly Action<Scene> _loadSceneCallback;
    
    private SceneManager(IReadOnlyCollection<Scene> scenes, Action<Scene> loadSceneCallback) {
        ActiveScene = scenes.First().name;
        _scenes = scenes.ToDictionary(s => s.name, s => s);
        _loadSceneCallback = loadSceneCallback;
    }

    public static void Init(List<Scene> scenes, Action<Scene> loadSceneCallback) {
        if (_self != null) {
            throw new Exception("There can only be one instance of the SceneManager!");
        }

        _self = new SceneManager(scenes, loadSceneCallback);
        LoadScene(ActiveScene);
    }

    public static void LoadScene(string name) {
        if (!_self._scenes.ContainsKey(name)) {
            throw new ArgumentException($"Unable to load scene with the name: {name} as there is no scene with that name");
        }
        
        ActiveScene = name;
        _self._loadSceneCallback.Invoke(_self._scenes[ActiveScene]);
    }
}