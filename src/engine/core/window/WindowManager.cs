using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.window; 

public class WindowManager {
    private static WindowManager _self = null!;

    private readonly nint _window;
    private readonly GameSettings _settings;

    private WindowManager(nint window, GameSettings settings) {
        _window = window;
        _settings = settings;
    }

    public static void Init(nint window, GameSettings settings) {
        if (_self != null) {
            throw new Exception("There can only be one Window Manager!");
        }

        _self = new WindowManager(window, settings);
    }

    public static void SetResolution(Vector2 resolution) {
        SDL.SDL_SetWindowSize(_self._window, (int)resolution.x, (int)resolution.y);
        _self._settings.width = (int)resolution.x;
        _self._settings.height = (int)resolution.y;
    }
    
    public static Vector2 CurrentResolution => new(_self._settings.width, _self._settings.height);
}