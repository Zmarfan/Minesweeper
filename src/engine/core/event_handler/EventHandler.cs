using SDL2;
using Worms.engine.core.input.listener;
using Worms.engine.data;
using Worms.engine.logger;

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
        try {
            HandleNewEvents();
        }
        catch (Exception e) {
            Logger.Error(e, "There was an issue with the event handling this frame");
        }
    }

    private void HandleNewEvents() {
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

                    if (SdlInputCodeToButton.SCANCODE_TO_BUTTON.TryGetValue(e.key.keysym.scancode, out Button value)) {
                        KeyDownEvent?.Invoke(value);
                    }
                    break;
                }
                case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN: {
                    if (SdlInputCodeToButton.MOUSE_BUTTON_TO_BUTTON.TryGetValue(e.button.button, out Button value)) {
                        KeyDownEvent?.Invoke(value);
                    }
                    break;
                }
                case SDL.SDL_EventType.SDL_KEYUP: {
                    if (SdlInputCodeToButton.SCANCODE_TO_BUTTON.TryGetValue(e.key.keysym.scancode, out Button value)) {
                        KeyUpEvent?.Invoke(value);
                    }
                    break;
                }
                case SDL.SDL_EventType.SDL_MOUSEBUTTONUP: {
                    if (SdlInputCodeToButton.MOUSE_BUTTON_TO_BUTTON.TryGetValue(e.button.button, out Button value)) {
                        KeyUpEvent?.Invoke(value);
                    }
                    break;
                }
                case SDL.SDL_EventType.SDL_MOUSEMOTION: {
                    float relativeXPosition = e.motion.x / (float)_settings.width;
                    float relativeYPosition = (_settings.height - e.motion.y) / (float)_settings.height;
                    MouseMovementEvent?.Invoke(new Vector2(relativeXPosition, relativeYPosition), new Vector2(e.motion.xrel, -e.motion.yrel));
                    break;
                }
                default:
                    return;
            }
        }
    }

    private static bool IsEnterFullScreen(SDL.SDL_Keysym key) {
        return key is { scancode: SDL.SDL_Scancode.SDL_SCANCODE_RETURN, mod: SDL.SDL_Keymod.KMOD_LALT };
    }
}