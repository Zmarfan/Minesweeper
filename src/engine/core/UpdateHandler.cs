using SDL2;
using Worms.engine.game_object;

namespace Worms.engine.core; 

public class UpdateHandler {
    private GameSettings _settings;
    private ulong _now;
    private ulong _last;

    public UpdateHandler(GameSettings settings) {
        _settings = settings;
        _now = SDL.SDL_GetPerformanceCounter();
        _last = 0;
    }
    
    public void Update() {
        float deltaTime = GetDeltaTime();
        _settings.camera.Size += deltaTime * 0.00005f;
    }

    private float GetDeltaTime() {
        _last = _now;
        _now = SDL.SDL_GetPerformanceCounter();
        return (float)(_now - _last) * 1000 / SDL.SDL_GetPerformanceFrequency();
    }
}