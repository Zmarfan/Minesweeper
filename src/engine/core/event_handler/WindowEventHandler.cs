using SDL2;

namespace Worms.engine.core.event_handler; 

public class WindowEventHandler {
    private readonly GameSettings _settings;

    public WindowEventHandler(GameSettings settings) {
        _settings = settings;
    }

    public void HandleEvent(SDL.SDL_WindowEvent e) {
        if (e.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED) {
            _settings.width = e.data1;
            _settings.height = e.data2;
        }
    }
}