using Worms.engine.camera;
using Worms.engine.game_object;

namespace Worms.engine.scene; 

public class Scene {
    public readonly string name;
    public Action<Camera> CameraInitializer { get; }
    private readonly Func<GameObject> _worldGameObjectRootProvider;
    private readonly Func<GameObject> _screenGameObjectRootProvider;

    public Scene(string name, Func<GameObject> worldGameObjectRootProvider, Func<GameObject> screenGameObjectRootProvider, Action<Camera>? cameraInitializer = null) {
        CameraInitializer = cameraInitializer ?? (_ => { });
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