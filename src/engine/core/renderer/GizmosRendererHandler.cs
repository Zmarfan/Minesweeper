using SDL2;
using Worms.engine.core.gizmos;
using Worms.engine.game_object.scripts;

namespace Worms.engine.core.renderer; 

public class GizmosRendererHandler {
    private readonly IntPtr _renderer;
    private readonly GameSettings _settings;

    public GizmosRendererHandler(nint renderer, GameSettings settings) {
        _renderer = renderer;
        _settings = settings;
    }

    public void RenderGizmos(List<Script> scripts) {
        foreach (Script script in scripts) {
            Gizmos.matrix = script.Transform.LocalToWorldMatrix;
            script.OnDrawGizmos();
            while (Gizmos.GIZMOS_OBJECTS.Count > 0) {
                GizmosObject gizmos = Gizmos.GIZMOS_OBJECTS.Dequeue();
                SDL.SDL_SetRenderDrawColor(_renderer, gizmos.color.Rbyte, gizmos.color.Gbyte, gizmos.color.Bbyte, gizmos.color.Abyte);
                gizmos.Render(_renderer, _settings);
            }
        }
    }
}