using SDL2;
using Worms.engine.core.game_object_handler;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.logger;
using Worms.engine.scene;

namespace Worms.engine.core.renderer; 

public class RendererHandler {
    public const string DEFAULT_SORTING_LAYER = "Default";

    private readonly IntPtr _renderer;
    private readonly SceneData _sceneData;
    private readonly List<string> _sortLayers = new() { DEFAULT_SORTING_LAYER };

    public RendererHandler(IntPtr renderer, GameSettings settings, SceneData sceneData) {
        _renderer = renderer;
        _sceneData = sceneData;
        _sortLayers.AddRange(settings.sortLayers);
    }

    public void Render(Dictionary<GameObject, TrackObject> objects) {
        IEnumerable<TextureRenderer> textureRenderers = objects
            .Values
            .Where(obj => obj.isActive)
            .SelectMany(obj => obj.TextureRenderers)
            .Where(tr => tr.IsActive)
            .OrderByDescending(tr => _sortLayers.IndexOf(tr.sortingLayer))
            .ThenByDescending(tr => tr.orderInLayer);
        
        foreach (TextureRenderer tr in textureRenderers) {
            try {
                TransformationMatrix matrix = objects[tr.gameObject].isWorld ? _sceneData.camera.WorldToScreenMatrix : _sceneData.camera.UiToScreenMatrix;
                TextureRendererHandler.RenderTexture(_renderer, tr, matrix);
            }
            catch (ArgumentException e) {
                Logger.Error(e);
            }
        }
    }
}