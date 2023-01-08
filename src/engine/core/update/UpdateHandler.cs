using SDL2;
using Worms.engine.camera;
using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.engine.core.update; 

public class UpdateHandler {
    private readonly Camera _camera;
    private readonly GameObjectHandler _gameObjectHandler;
    private ulong _now;
    private ulong _last;

    public UpdateHandler(GameObjectHandler gameObjectHandler, Camera camera) {
        _gameObjectHandler = gameObjectHandler;
        _camera = camera;
        _now = SDL.SDL_GetPerformanceCounter();
        _last = 0;
        _camera.Awake();
    }
    
    public void Awake() {
        _gameObjectHandler.AllScripts.ForEach(static script => {
            if (!script.HasRunAwake) {
                script.Awake();
                script.HasRunAwake = true;
            }
        });
    }
    
    public void Start() {
        _gameObjectHandler.AllActiveGameObjectScripts.ForEach(static script => {
            if (script is { IsActive: true, HasRunStart: false }) {
                script.Start();
                script.HasRunStart = true;
            }
        });
    }
    
    public void Update() {
        float deltaTime = GetDeltaTime();
        foreach (Script script in _gameObjectHandler.AllActiveGameObjectScripts) {
            if (script.IsActive) {
                script.Update(deltaTime);
            }
        }
        _camera.Update(deltaTime);
        _gameObjectHandler.DestroyObjects();
    }

    private float GetDeltaTime() {
        _last = _now;
        _now = SDL.SDL_GetPerformanceCounter();
        return (float)(_now - _last) * 1000 / SDL.SDL_GetPerformanceFrequency() * 0.001f ;
    }
}