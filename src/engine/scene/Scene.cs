using Worms.engine.camera;
using Worms.engine.game_object;

namespace Worms.engine.scene; 

public class Scene {
    public readonly string name;
    private readonly Func<Camera> _cameraProvider;
    private readonly Func<GameObject> _worldGameObjectRootProvider;

    public Scene(string name, Func<Camera> cameraProvider, Func<GameObject> worldGameObjectRootProvider) {
        this.name = name;
        _cameraProvider = cameraProvider;
        _worldGameObjectRootProvider = worldGameObjectRootProvider;
    }

    public Camera CreateCamera() {
        return _cameraProvider.Invoke();
    }

    public GameObject CreateWorldGameObjectRoot() {
        return _worldGameObjectRootProvider.Invoke();
    } 
}