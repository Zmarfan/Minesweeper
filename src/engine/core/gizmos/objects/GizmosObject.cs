using GameEngine.engine.data;

namespace GameEngine.engine.core.gizmos.objects; 

public interface IGizmosObject {
    Color GetColor();
    void Render(nint renderer, TransformationMatrix worldToScreenMatrix);
}