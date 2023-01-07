using SDL2;
using Worms.engine.data;
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
        if (_settings.camera.Size > 2) {
            _settings.camera.Size = 1;
            _settings.camera.Position = new Vector2(-500, 0);
        }
        _settings.camera.Position += Vector2.Right() * deltaTime * 0.05f;
    }

    private float GetDeltaTime() {
        _last = _now;
        _now = SDL.SDL_GetPerformanceCounter();
        return (float)(_now - _last) * 1000 / SDL.SDL_GetPerformanceFrequency();
    }
}