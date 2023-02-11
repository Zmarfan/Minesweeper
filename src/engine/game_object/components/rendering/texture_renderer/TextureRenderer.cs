using Worms.engine.data;

namespace Worms.engine.game_object.components.rendering.texture_renderer;

public class TextureRenderer : RenderComponent {
    public Texture texture;
    public bool flipX;
    public bool flipY;
    
    public TextureRenderer(
        bool isActive,
        Texture texture,
        string sortingLayer,
        int orderInLayer,
        Color color,
        bool flipX,
        bool flipY
    ) : base(isActive, sortingLayer, orderInLayer, color) {
        this.texture = texture;
        this.flipX = flipX;
        this.flipY = flipY;
    }
}