using SDL2;
using Worms.engine.camera;
using Worms.engine.core.game_object_handler;
using Worms.engine.core.gizmos;
using Worms.engine.core.gizmos.objects;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components;
using Worms.engine.scene;

namespace Worms.engine.core.renderer; 

public class GizmosRendererHandler {
    private readonly nint _renderer;

    public GizmosRendererHandler(nint renderer) {
        _renderer = renderer;
    }

    public void RenderGizmos(Dictionary<GameObject, TrackObject> objects) {
        IEnumerable<ToggleComponent> components = objects
            .Values
            .Where(obj => obj.isActive)
            .SelectMany(obj => obj.toggleComponents)
            .Where(script => script.IsActive);
        
        foreach (ToggleComponent component in components) {
            Gizmos.matrix = TransformationMatrix.Identity();
            component.OnDrawGizmos();
            
            while (Gizmos.GIZMOS_OBJECTS.Count > 0) {
                IGizmosObject gizmos = Gizmos.GIZMOS_OBJECTS.Dequeue();
                SDL.SDL_SetRenderDrawColor(_renderer, gizmos.GetColor().Rbyte, gizmos.GetColor().Gbyte, gizmos.GetColor().Bbyte, gizmos.GetColor().Abyte);
                gizmos.Render(_renderer, objects[component.gameObject].isWorld ? Camera.Main.WorldToScreenMatrix : Camera.Main.UiToScreenMatrix);
            }
        }
    }
}