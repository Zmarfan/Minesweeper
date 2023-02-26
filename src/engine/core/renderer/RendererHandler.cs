using Worms.engine.camera;
using Worms.engine.core.game_object_handler;
using Worms.engine.core.renderer.font;
using Worms.engine.core.renderer.textures;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.rendering;
using Worms.engine.game_object.components.rendering.text_renderer;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.engine.logger;
using Worms.engine.scene;

namespace Worms.engine.core.renderer; 

public class RendererHandler {
    public const string DEFAULT_SORTING_LAYER = "Default";

    private readonly nint _renderer;
    private readonly FontHandler _fontHandler;
    private readonly List<string> _sortLayers = new() { DEFAULT_SORTING_LAYER };

    public RendererHandler(nint renderer, FontHandler fontHandler, GameSettings settings) {
        _renderer = renderer;
        _fontHandler = fontHandler;
        _sortLayers.AddRange(settings.sortLayers);
    }

    public void Render(Dictionary<GameObject, TrackObject> objects) {
        IEnumerable<RenderComponent> renderComponents = objects
            .Values
            .Where(obj => obj.isActive)
            .SelectMany(obj => obj.TextureRenderers)
            .Where(tr => tr.IsActive)
            .OrderByDescending(tr => _sortLayers.IndexOf(tr.sortingLayer))
            .ThenByDescending(tr => tr.orderInLayer);
        
        foreach (RenderComponent renderComponent in renderComponents) {
            try {
                TransformationMatrix matrix = objects[renderComponent.gameObject].isWorld ? Camera.Main.WorldToScreenMatrix : Camera.Main.UiToScreenMatrix;
                switch (renderComponent) {
                    case TextureRenderer texture:
                        TextureRendererHandler.RenderTexture(_renderer, texture, matrix);
                        break;
                    case TextRenderer text:
                        TextRendererHandler.RenderText(_renderer, _fontHandler.fonts[text.Font], text, matrix);
                        break;
                }
            }
            catch (ArgumentException e) {
                Logger.Error(e);
            }
        }
    }
}