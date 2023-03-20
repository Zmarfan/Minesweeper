using GameEngine.engine.core.input.listener;
using GameEngine.engine.data;

namespace GameEngine.engine.core.event_handler;

internal delegate void EventVoidDelegate();
internal delegate void ButtonEventDelegate(Button button);
internal delegate void MouseMovementEventDelegate(Vector2 position, Vector2 direction);