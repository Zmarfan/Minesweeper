using SDL2;

namespace Worms.engine.core; 

public class EventHandler {
    private readonly Action _quitCallback;
    
    public EventHandler(Action quitCallback) {
        _quitCallback = quitCallback;
    }
    
    public void HandleEvents() {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
        {
            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    _quitCallback.Invoke();
                    break;
            }
        }
    }
}