using Worms.engine.data;

namespace Worms.engine.core.window; 

public class WindowManager {
    private static WindowManager _self = null!;

    private readonly GameSettings _settings;

    private WindowManager(GameSettings settings) {
        _settings = settings;
    }

    public static void Init(GameSettings settings) {
        if (_self != null) {
            throw new Exception("There can only be one Window Manager!");
        }

        _self = new WindowManager(settings);
    }

    public static Vector2 CurrentResolution => new(_self._settings.width, _self._settings.height);
}