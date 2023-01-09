using Worms.engine.core.input.listener;
using Worms.engine.data;

namespace Worms.engine.core.event_handler; 

public delegate void EventVoidDelegate();
public delegate void ButtonEventDelegate(Button button);
public delegate void MouseMovementEventDelegate(Vector2 position, Vector2 direction);