using Worms.engine.core.renderer;
using Worms.engine.data;
using Worms.engine.game_object.components.texture_renderer;

namespace Worms.engine.game_object.components.colliders; 

public class TextureColliderBuilder {
    private bool _isActive = true;
    private bool _isTrigger = true;
    private readonly Texture _texture;
    private string _sortingLayer = TextureRendererHandler.DEFAULT_SORTING_LAYER;
    private int _sortOrder = 0;
    private Color _color = Color.WHITE;
    private bool _flipX;
    private bool _flipY;

    public TextureColliderBuilder(Texture texture) {
        _texture = texture;
    }

    public static TextureColliderBuilder Builder(Texture texture) {
        return new TextureColliderBuilder(texture);
    }

    public TextureCollider Build() {
        return new TextureCollider(
            _isActive,
            _isTrigger,
            new TextureRenderer(true, _texture, _sortingLayer, _sortOrder, _color, _flipX, _flipY)
        );
    }
    
    public TextureColliderBuilder SetIsActive(bool isActive) {
        _isActive = isActive;
        return this;
    }
    
    public TextureColliderBuilder SetIsTrigger(bool isTrigger) {
        _isTrigger = isTrigger;
        return this;
    }

    public TextureColliderBuilder SetSortingLayer(string sortingLayer) {
        _sortingLayer = sortingLayer;
        return this;
    }
    
    public TextureColliderBuilder SetSortingOrder(int sortingOrder) {
        _sortOrder = sortingOrder;
        return this;
    }
    
    public TextureColliderBuilder SetColor(Color color) {
        _color = color;
        return this;
    }
    
    public TextureColliderBuilder SetFlipX(bool flipX) {
        _flipX = flipX;
        return this;
    }
    
    public TextureColliderBuilder SetFlipY(bool flipY) {
        _flipY = flipY;
        return this;
    }
}