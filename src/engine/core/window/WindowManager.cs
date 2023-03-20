using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.window; 

public class WindowManager {
    public delegate void ResolutionChangedDelegate(Vector2Int resolution);
    public static event ResolutionChangedDelegate? ResolutionChangedEvent;
    
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

    public static void SetResolution(Vector2Int resolution) {
        if (resolution.x <= 0 || resolution.y <= 0) {
            throw new ArgumentException("Resolution must be > 0 per x and y axis");
        }
        SDL.SDL_SetWindowSize(_self._window, resolution.x, resolution.y);
        _self._settings.width = resolution.x;
        _self._settings.height = resolution.y;
        ResolutionChangedEvent?.Invoke(resolution);
    }
    
    public static Vector2Int CurrentResolution => new(_self._settings.width, _self._settings.height);
}