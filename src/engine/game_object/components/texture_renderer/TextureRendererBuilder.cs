using Worms.engine.core.renderer;
using Worms.engine.data;

namespace Worms.engine.game_object.components.texture_renderer; 

public class TextureRendererBuilder {
    private bool _isActive = true;
    private readonly string _textureSrc;
    private string _sortingLayer = TextureRendererHandler.DEFAULT_SORTING_LAYER;
    private int _sortOrder = 0;
    private Color _color = Color.White();
    private bool _flipX;
    private bool _flipY;

    public TextureRendererBuilder(string textureSrc) {
        _textureSrc = textureSrc;
    }

    public static TextureRendererBuilder Builder(string textureSrc) {
        return new TextureRendererBuilder(textureSrc);
    }

    public TextureRenderer Build() {
        return new TextureRenderer(_isActive, _textureSrc, _sortingLayer, _sortOrder, _color, _flipX, _flipY);
    }
    
    public TextureRendererBuilder SetIsActive(bool isActive) {
        _isActive = isActive;
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