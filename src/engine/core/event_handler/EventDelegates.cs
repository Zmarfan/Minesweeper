using GameEngine.engine.core.input.listener;
using GameEngine.engine.data;

namespace GameEngine.engine.core.event_handler;

public delegate void EventVoidDelegate();
public delegate void ButtonEventDelegate(Button button);
public delegate void MouseMovementEventDelegate(Vector2 position, Vector2 direction);