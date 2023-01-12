using SDL2;
using Worms.engine.camera;
using Worms.engine.core.input;
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
            try {
                if (!script.HasRunAwake) {
                    script.Awake();
                    script.HasRunAwake = true;
                }
            }
            catch (Exception e) {
                Console.WriteLine($"An exception occured in {script} during the Awake callback: {e}");
            }
        });
    }
    
    public void Start() {
        _gameObjectHandler.AllActiveGameObjectScripts.ForEach(static script => {
            try {
                if (script is { IsActive: true, HasRunStart: false }) {
                    script.Start();
                    script.HasRunStart = true;
                }
            }
            catch (Exception e) {
                Console.WriteLine($"An exception occured in {script} during the Start callback: {e}");
            }
        });
    }
    
    public void Update() {
        float deltaTime = GetDeltaTime();
        Input.Update(deltaTime);
        foreach (Script script in _gameObjectHandler.AllActiveGameObjectScripts) {
            try {
                if (script.IsActive) {
                    script.Update(deltaTime);
                }
            }
            catch (Exception e) {
                Console.WriteLine($"An exception occured in {script} during the Update callback: {e}");
            }
        }
        _camera.Update(deltaTime);
        _gameObjectHandler.DestroyObjects();
        Input.FrameReset();
    }

    private float GetDeltaTime() {
        _last = _now;
        _now = SDL.SDL_GetPerformanceCounter();
        return (float)(_now - _last) * 1000 / SDL.SDL_GetPerformanceFrequency() * 0.001f ;
    }
}