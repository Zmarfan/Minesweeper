using Worms.engine.data;

namespace Worms.engine.game_object.components.texture_renderer;

public class TextureRenderer : ToggleComponent {
    public Texture texture;
    public string sortingLayer;
    public int orderInLayer;
    public Color color;
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
    ) : base(isActive) {
        this.texture = texture;
        this.sortingLayer = sortingLayer;
        this.orderInLayer = orderInLayer;
        this.color = color;
        this.flipX = flipX;
        this.flipY = flipY;
    }
}