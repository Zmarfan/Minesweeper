using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.event_handler; 

public delegate void EventVoidDelegate();
public delegate void KeyDownEventDelegate(SDL.SDL_Scancode scancode);
public delegate void MouseMovementEventDelegate(Vector2 position, Vector2 direction);