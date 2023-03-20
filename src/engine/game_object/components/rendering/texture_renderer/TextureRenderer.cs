using GameEngine.engine.data;

namespace GameEngine.engine.game_object.components.rendering.texture_renderer;

public class TextureRenderer : RenderComponent {
    public Texture texture;
    public bool flipX;
    public bool flipY;
    
    public TextureRenderer(
        bool isActive,
        string name,
        Texture texture,
        string sortingLayer,
        int orderInLayer,
        Color color,
        bool flipX,
        bool flipY
    ) : base(sortingLayer, orderInLayer, color, isActive, name) {
        this.texture = texture;
        this.flipX = flipX;
        this.flipY = flipY;
    }
}