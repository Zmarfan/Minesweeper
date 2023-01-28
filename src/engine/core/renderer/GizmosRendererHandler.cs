using SDL2;
using Worms.engine.core.game_object_handler;
using Worms.engine.core.gizmos;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.scripts;
using Worms.engine.scene;

namespace Worms.engine.core.renderer; 

public class GizmosRendererHandler {
    private readonly IntPtr _renderer;
    private readonly SceneData _sceneData;

    public GizmosRendererHandler(nint renderer, SceneData sceneData) {
        _renderer = renderer;
        _sceneData = sceneData;
    }

    public void RenderGizmos(Dictionary<GameObject, TrackObject> objects) {
        IEnumerable<Script> scripts = objects
            .Values
            .Where(obj => obj.isActive)
            .SelectMany(obj => obj.scripts)
            .Where(script => script.IsActive);
        
        foreach (Script script in scripts) {
            Gizmos.matrix = TransformationMatrix.Identity();
            script.OnDrawGizmos();
            while (Gizmos.GIZMOS_OBJECTS.Count > 0) {
                GizmosObject gizmos = Gizmos.GIZMOS_OBJECTS.Dequeue();
                SDL.SDL_SetRenderDrawColor(_renderer, gizmos.color.Rbyte, gizmos.color.Gbyte, gizmos.color.Bbyte, gizmos.color.Abyte);
                gizmos.Render(_renderer, objects[script.gameObject].isWorld ? _sceneData.camera.WorldToScreenMatrix : _sceneData.camera.UiToScreenMatrix);
            }
        }
    }
}