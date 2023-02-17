using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.gizmos; 

public class GizmosPoints : GizmosObject {
    private readonly IEnumerable<Vector2> _points;
    
    public GizmosPoints(IEnumerable<Vector2> points, Color color) : base(color) {
        _points = points;
    }

    public override void Render(nint renderer, TransformationMatrix worldToScreenMatrix) {
        SDL.SDL_FPoint[] formattedPoints = _points
            .Select(worldToScreenMatrix.ConvertPoint)
            .Select(p => new SDL.SDL_FPoint { x = p.x, y = p.y })
            .ToArray();
        if (SDL.SDL_RenderDrawPointsF(renderer, formattedPoints, formattedPoints.Length) != 0) {
            throw new Exception($"Unable to draw gizmo points due to: {SDL.SDL_GetError()}");
        }
    }
}