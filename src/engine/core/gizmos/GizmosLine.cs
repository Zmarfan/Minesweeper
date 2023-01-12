using SDL2;
using Worms.engine.camera;
using Worms.engine.core.renderer;
using Worms.engine.data;

namespace Worms.engine.core.gizmos; 

public class GizmosLine : GizmosObject {
    private readonly Vector2 _from;
    private readonly Vector2 _to;

    public GizmosLine(Vector2 from, Vector2 to, Color color) : base(color) {
        _from = from;
        _to = to;
    }

    public override void Render(IntPtr renderer, GameSettings settings) {
        Vector2 fromScreen = WorldToScreenCalculator.WorldToScreenPosition(_from, settings);
        Vector2 toScreen = WorldToScreenCalculator.WorldToScreenPosition(_to, settings);
        SDL.SDL_RenderDrawLineF(renderer, fromScreen.x, fromScreen.y, toScreen.x, toScreen.y);
    }
}