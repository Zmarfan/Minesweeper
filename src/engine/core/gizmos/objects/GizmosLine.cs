using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.gizmos.objects; 

public class GizmosLine : GizmosObject {
    private readonly Vector2 _from;
    private readonly Vector2 _to;

    public GizmosLine(Vector2 from, Vector2 to, Color color) : base(color) {
        _from = from;
        _to = to;
    }

    public override void Render(nint renderer, TransformationMatrix worldToScreenMatrix) {
        Vector2 fromScreen = worldToScreenMatrix.ConvertPoint(_from);
        Vector2 toScreen = worldToScreenMatrix.ConvertPoint(_to);
        SDL.SDL_RenderDrawLineF(renderer, fromScreen.x, fromScreen.y, toScreen.x, toScreen.y);
    }
}