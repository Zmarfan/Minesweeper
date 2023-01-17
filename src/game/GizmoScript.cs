using Worms.engine.core.gizmos;
using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class GizmoScript : Script {
    public GizmoScript() : base(true) {
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawRay(Transform.Position, Transform.Up * 1000f, Color.GREEN);
        Gizmos.DrawRay(Transform.Position, Transform.Right * 1000f, Color.BLUE);
        Gizmos.DrawRay(Transform.Position, Transform.Left * 1000f, Color.BLACK);
        Gizmos.DrawRay(Transform.Position, Transform.Down * 1000f, Color.WHITE);
        Gizmos.DrawCircle(Transform.Position, 500f, new Color(1f, 0.5f, 0.75f));
        Gizmos.DrawIcon(Transform.Position, new Color(1f, 1f, 0.75f));
    }
}