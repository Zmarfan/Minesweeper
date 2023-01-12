using Worms.engine.camera;
using Worms.engine.data;

namespace Worms.engine.core.gizmos; 

public abstract class GizmosObject {
    public readonly Color color;

    protected GizmosObject(Color color) {
        this.color = color;
    }

    public abstract void Render(IntPtr renderer, GameSettings settings);
}