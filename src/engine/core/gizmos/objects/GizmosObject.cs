using GameEngine.engine.data;

namespace GameEngine.engine.core.gizmos.objects; 

internal interface IGizmosObject {
    Color GetColor();
    void Render(nint renderer, TransformationMatrix worldToScreenMatrix);
}