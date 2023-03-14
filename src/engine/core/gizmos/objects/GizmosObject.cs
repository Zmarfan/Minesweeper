using Worms.engine.data;

namespace Worms.engine.core.gizmos.objects; 

public interface IGizmosObject {
    Color GetColor();
    void Render(nint renderer, TransformationMatrix worldToScreenMatrix);
}