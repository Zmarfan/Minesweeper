using Worms.engine.core.update;
using Worms.engine.data;

namespace Worms.engine.game_object.components.physics; 

public class RigidBody : ToggleComponent {
    public RigidBody(bool isActive) : base(isActive) {
    }
}