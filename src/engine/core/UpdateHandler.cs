using SDL2;

namespace Worms.engine.core; 

public class UpdateHandler {
    private ulong _now;
    private ulong _last;

    public UpdateHandler() {
        _now = SDL.SDL_GetPerformanceCounter();
        _last = 0;
    }
    
    public void Update() {
        float deltaTime = GetDeltaTime();
    }

    private float GetDeltaTime() {
        _last = _now;
        _now = SDL.SDL_GetPerformanceCounter();
        return (float)(_now - _last) * 1000 / SDL.SDL_GetPerformanceFrequency();
    }
}