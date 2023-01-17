using Worms.engine.core.gizmos;
using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class GizmoScript : Script {
    public GizmoScript() : base(true) {
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawRay(Transform.LocalPosition, Vector2.Up() * 1000f, new Color(1f, 0.5f, 0.75f));
        Gizmos.DrawCircle(Transform.LocalPosition, 500f, new Color(1f, 0.5f, 0.75f));
        Gizmos.DrawIcon(Transform.LocalPosition, new Color(1f, 1f, 0.75f));
    }
}