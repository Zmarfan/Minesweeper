using SDL2;
using Worms.engine.camera;
using Worms.engine.core.input;
using Worms.engine.game_object.scripts;

namespace Worms.engine.core.update; 

public class UpdateHandler {
    private const float FIXED_UPDATE_CYCLE_TIME = 0.02f;
    
    private readonly Camera _camera;
    private readonly GameObjectHandler _gameObjectHandler;
    private ulong _now;
    private ulong _last;
    private float _fixedUpdateAcc;
    private float _deltaTime;

    public UpdateHandler(GameObjectHandler gameObjectHandler, Camera camera) {
        _gameObjectHandler = gameObjectHandler;
        _camera = camera;
        _now = SDL.SDL_GetPerformanceCounter();
        _last = 0;
        _fixedUpdateAcc = 0;
        _camera.Awake();
    }
    
    public void Awake() {
        _gameObjectHandler.AllScripts.ForEach(static script => {
            try {
                if (!script.HasRunAwake) {
                    script.Awake();
                }
            }
            catch (Exception e) {
                Console.WriteLine($"An exception occured in {script} during the Awake callback: {e}");
            }
            script.HasRunAwake = true;
        });
    }
    
    public void Start() {
        _gameObjectHandler.AllActiveGameObjectScripts.ForEach(static script => {
            try {
                if (script is { IsActive: true, HasRunStart: false }) {
                    script.Start();
                }
            }
            catch (Exception e) {
                Console.WriteLine($"An exception occured in {script} during the Start callback: {e}");
            }
            script.HasRunStart = true;
        });
    }
    
    public void UpdateLoops() {
        UpdateFrameTimeData();
        while (_fixedUpdateAcc > FIXED_UPDATE_CYCLE_TIME) {
            FixedUpdate();
            _fixedUpdateAcc -= FIXED_UPDATE_CYCLE_TIME;
        }
        Update();
    }

    public void EndOfFrameCleanUp() {
        _gameObjectHandler.EndOfFrameCleanup();
        Input.FrameReset();
    }
    
    private void FixedUpdate() {
        foreach (Script script in _gameObjectHandler.AllActiveGameObjectScripts) {
            try {
                if (script.IsActive) {
                    script.FixedUpdate(FIXED_UPDATE_CYCLE_TIME);
                }
            }
            catch (Exception e) {
                Console.WriteLine($"An exception occured in {script} during the Fixed Update callback: {e}");
            }
        }
    }
    
    private void Update() {
        Input.Update(_deltaTime);
        
        foreach (Script script in _gameObjectHandler.AllActiveGameObjectScripts.ToList()) {
            try {
                if (script.IsActive) {
                    script.Update(_deltaTime);
                }
            }
            catch (Exception e) {
                Console.WriteLine($"An exception occured in {script} during the Update callback: {e}");
            }
        }
        _camera.Update(_deltaTime);
    }
    
    private void UpdateFrameTimeData() {
        _last = _now;
        _now = SDL.SDL_GetPerformanceCounter();
        _deltaTime = (float)(_now - _last) * 1000 / SDL.SDL_GetPerformanceFrequency() * 0.001f;
        _fixedUpdateAcc += _deltaTime;
    }
}