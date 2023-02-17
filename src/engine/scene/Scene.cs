using Worms.engine.game_object;

namespace Worms.engine.scene; 

public class Scene {
    public readonly string name;
    private readonly Func<GameObject> _worldGameObjectRootProvider;
    private readonly Func<GameObject> _screenGameObjectRootProvider;

    public Scene(string name, Func<GameObject> worldGameObjectRootProvider, Func<GameObject> screenGameObjectRootProvider) {
        this.name = name;
        _worldGameObjectRootProvider = worldGameObjectRootProvider;
        _screenGameObjectRootProvider = screenGameObjectRootProvider;
    }

    public GameObject CreateWorldGameObjectRoot() {
        return _worldGameObjectRootProvider.Invoke();
    }

    public GameObject CreateSceneGameObjectRoot() {
        return _screenGameObjectRootProvider.Invoke();
    }
}