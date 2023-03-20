using GameEngine.engine.core.renderer;
using GameEngine.engine.data;
using GameEngine.engine.game_object.components.rendering.texture_renderer;

namespace GameEngine.engine.game_object.components.physics.colliders; 

public class TextureColliderBuilder {
    private bool _isActive = true;
    private string _name = "textureCollider";
    private ColliderState _state = ColliderState.TRIGGER;
    private readonly Texture _texture;
    private string _sortingLayer = RendererHandler.DEFAULT_SORTING_LAYER;
    private int _sortOrder;
    private Color _color = Color.WHITE;
    private bool _flipX;
    private bool _flipY;

    private TextureColliderBuilder(Texture texture) {
        _texture = texture;
    }

    public static TextureColliderBuilder Builder(Texture texture) {
        return new TextureColliderBuilder(texture);
    }

    public TextureCollider Build() {
        return new TextureCollider(
            _isActive,
            _name,
            _state,
            new TextureRenderer(true, "textureColliderRenderer", _texture, _sortingLayer, _sortOrder, _color, _flipX, _flipY)
        );
    }
    
    public TextureColliderBuilder SetIsActive(bool isActive) {
        _isActive = isActive;
        return this;
    }
    
    public TextureColliderBuilder SetName(string name) {
        _name = name;
        return this;
    }
    
    public TextureColliderBuilder SetState(ColliderState state) {
        _state = state;
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