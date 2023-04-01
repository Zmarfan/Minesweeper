using GameEngine.engine.core.renderer;
using GameEngine.engine.data;
using Color = GameEngine.engine.data.Color;

namespace GameEngine.engine.game_object.components.rendering.texture_renderer; 

public class TextureRendererBuilder {
    private bool _isActive = true;
    private string _name = "textureRenderer";
    private readonly Texture _texture;
    private string _sortingLayer = RendererHandler.DEFAULT_SORTING_LAYER;
    private int _sortOrder;
    private Color _color = Color.WHITE;
    private bool _flipX;
    private bool _flipY;

    public TextureRendererBuilder(Texture texture) {
        _texture = texture;
    }

    public static TextureRendererBuilder Builder(Texture texture) {
        return new TextureRendererBuilder(texture);
    }

    public TextureRenderer Build() {
        return new TextureRenderer(_isActive, _name, _texture, _sortingLayer, _sortOrder, _color, _flipX, _flipY);
    }
    
    public TextureRendererBuilder SetIsActive(bool isActive) {
        _isActive = isActive;
        return this;
    }
    
    public TextureRendererBuilder SetName(string name) {
        _name = name;
        return this;
    }
    
    public TextureRendererBuilder SetSortingLayer(string sortingLayer) {
        _sortingLayer = sortingLayer;
        return this;
    }
    
    public TextureRendererBuilder SetSortingOrder(int sortingOrder) {
        _sortOrder = sortingOrder;
        return this;
    }
    
    public TextureRendererBuilder SetColor(Color color) {
        _color = color;
        return this;
    }
    
    public TextureRendererBuilder SetFlipX(bool flipX) {
        _flipX = flipX;
        return this;
    }
    
    public TextureRendererBuilder SetFlipY(bool flipY) {
        _flipY = flipY;
        return this;
    }
}