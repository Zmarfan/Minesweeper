using SDL2;

namespace Worms.engine.core.event_handler; 

public class EventHandler {
    public event EventVoidDelegate? QuitEvent;
    public event KeyDownEventDelegate? KeyDownEvent;
    public event KeyDownEventDelegate? KeyUpEvent;
    
    public readonly WindowEventHandler windowEventHandler;
    
    public EventHandler(GameSettings settings) {
        windowEventHandler = new WindowEventHandler(settings);
    }
    
    public void HandleEvents() {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
        {
            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    QuitEvent?.Invoke();
                    break;
                case SDL.SDL_EventType.SDL_WINDOWEVENT: {
                    windowEventHandler.HandleEvent(e.window);
                    break;
                }
                case SDL.SDL_EventType.SDL_KEYDOWN: {
                    KeyDownEvent?.Invoke(e.key.keysym.scancode);
                    break;
                }
                case SDL.SDL_EventType.SDL_KEYUP: {
                    KeyUpEvent?.Invoke(e.key.keysym.scancode);
                    break;
                }
            }
        }
    }
}