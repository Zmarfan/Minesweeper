using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.gizmos; 

public class GizmosPoint : GizmosObject {
    private readonly Vector2 _point;
    
    public GizmosPoint(Vector2 point, Color color) : base(color) {
        _point = point;
    }

    public override void Render(IntPtr renderer, TransformationMatrix worldToScreenMatrix) {
        Vector2 drawPoint = worldToScreenMatrix.ConvertPoint(_point);
        SDL.SDL_RenderDrawPointF(renderer, drawPoint.x, drawPoint.y);
    }
}