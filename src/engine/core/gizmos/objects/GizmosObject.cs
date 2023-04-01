using GameEngine.engine.data;
using Color = GameEngine.engine.data.Color;

namespace GameEngine.engine.core.gizmos.objects; 

internal interface IGizmosObject {
    Color GetColor();
    void Render(nint renderer, TransformationMatrix worldToScreenMatrix);
}