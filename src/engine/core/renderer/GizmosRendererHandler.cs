using SDL2;
using Worms.engine.camera;
using Worms.engine.core.gizmos;
using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.engine.core.renderer; 

public class GizmosRendererHandler {
    private readonly IntPtr _renderer;
    private readonly Camera _camera;

    public GizmosRendererHandler(nint renderer, Camera camera) {
        _renderer = renderer;
        _camera = camera;
    }

    public void RenderGizmos(List<Script> scripts) {
        foreach (Script script in scripts) {
            Gizmos.matrix = TransformationMatrix.Identity();
            script.OnDrawGizmos();
            while (Gizmos.GIZMOS_OBJECTS.Count > 0) {
                GizmosObject gizmos = Gizmos.GIZMOS_OBJECTS.Dequeue();
                SDL.SDL_SetRenderDrawColor(_renderer, gizmos.color.Rbyte, gizmos.color.Gbyte, gizmos.color.Bbyte, gizmos.color.Abyte);
                gizmos.Render(_renderer, _camera.WorldToScreenMatrix);
            }
        }
    }
}