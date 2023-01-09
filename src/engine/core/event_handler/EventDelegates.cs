using SDL2;

namespace Worms.engine.core.event_handler; 

public delegate void EventVoidDelegate();
public delegate void KeyDownEventDelegate(SDL.SDL_Scancode scancode);