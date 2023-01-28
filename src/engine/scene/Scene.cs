using Worms.engine.camera;
using Worms.engine.game_object;

namespace Worms.engine.scene; 

public class Scene {
    public readonly string name;
    private readonly Func<Camera> _cameraProvider;
    private readonly Func<GameObject> _worldGameObjectRootProvider;
    private readonly Func<GameObject> _screenGameObjectRootProvider;

    public Scene(string name, Func<Camera> cameraProvider, Func<GameObject> worldGameObjectRootProvider, Func<GameObject> screenGameObjectRootProvider) {
        this.name = name;
        _cameraProvider = cameraProvider;
        _worldGameObjectRootProvider = worldGameObjectRootProvider;
        _screenGameObjectRootProvider = screenGameObjectRootProvider;
    }

    public Camera CreateCamera() {
        return _cameraProvider.Invoke();
    }

    public GameObject CreateWorldGameObjectRoot() {
        return _worldGameObjectRootProvider.Invoke();
    }

    public GameObject CreateSceneGameObjectRoot() {
        return _screenGameObjectRootProvider.Invoke();
    }
}