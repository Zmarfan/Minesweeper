using Worms.engine.core.gizmos;
using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class GizmoScript : Script {
    public GizmoScript() : base(true) {
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawLine(Transform.Position, Transform.Position + Vector2.Up() * 1000f, new Color(1f, 0.5f, 0.75f));
        Gizmos.DrawLine(Transform.Position, Transform.Position + Vector2.Left() * 1000f, new Color(1f, 0.1f, 0.25f));
        Gizmos.DrawCircle(Transform.Position, 1000f, new Color(0f, 1f, 0.5f));
    }
}