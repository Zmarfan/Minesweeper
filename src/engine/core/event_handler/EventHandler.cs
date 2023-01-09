using SDL2;
using Worms.engine.core.input.listener;
using Worms.engine.data;

namespace Worms.engine.core.event_handler; 

public class EventHandler {
    public event EventVoidDelegate? QuitEvent;
    public event ButtonEventDelegate? KeyDownEvent;
    public event ButtonEventDelegate? KeyUpEvent;
    public event MouseMovementEventDelegate? MouseMovementEvent;
    public event EventVoidDelegate? ToggleFullscreenEvent;

    private readonly GameSettings _settings;
    
    public EventHandler(GameSettings settings) {
        _settings = settings;
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
                    if (e.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED) {
                        _settings.width = e.window.data1;
                        _settings.height = e.window.data2;
                    }
                    break;
                }
                case SDL.SDL_EventType.SDL_KEYDOWN: {
                    if (IsEnterFullScreen(e.key.keysym)) {
                        ToggleFullscreenEvent?.Invoke();
                    }
                    KeyDownEvent?.Invoke(SdlInputCodeToButton.SCANCODE_TO_BUTTON[e.key.keysym.scancode]);
                    break;
                }
                case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN: {
                    KeyDownEvent?.Invoke(SdlInputCodeToButton.MOUSE_BUTTON_TO_BUTTON[e.button.button]);
                    break;
                }
                case SDL.SDL_EventType.SDL_KEYUP: {
                    KeyUpEvent?.Invoke(SdlInputCodeToButton.SCANCODE_TO_BUTTON[e.key.keysym.scancode]);
                    break;
                }
                case SDL.SDL_EventType.SDL_MOUSEBUTTONUP: {
                    KeyUpEvent?.Invoke(SdlInputCodeToButton.MOUSE_BUTTON_TO_BUTTON[e.button.button]);
                    break;
                }
                case SDL.SDL_EventType.SDL_MOUSEMOTION: {
                    float relativeXPosition = e.motion.x / (float)_settings.width;
                    float relativeYPosition = (_settings.height - e.motion.y) / (float)_settings.height;
                    MouseMovementEvent?.Invoke(new Vector2(relativeXPosition, relativeYPosition), new Vector2(e.motion.xrel, -e.motion.yrel));
                    break;
                }
            }
        }
    }

    private static bool IsEnterFullScreen(SDL.SDL_Keysym key) {
        return key is { scancode: SDL.SDL_Scancode.SDL_SCANCODE_RETURN, mod: SDL.SDL_Keymod.KMOD_LALT };
    }
}