using Worms.engine.data;

namespace Worms.engine.game_object.components.texture_renderer;

public class TextureRenderer : ToggleComponent {
    private static readonly string ROOT_DIRECTORY = Directory.GetCurrentDirectory();
    
    public string textureSrc;
    public string sortingLayer;
    public int orderInLayer;
    public Color color;
    public bool flipX;
    public bool flipY;
    
    public TextureRenderer(
        bool isActive,
        string textureSrc,
        string sortingLayer,
        int orderInLayer,
        Color color,
        bool flipX,
        bool flipY
    ) : base(isActive) {
        this.textureSrc = $"{ROOT_DIRECTORY}\\{textureSrc}";
        this.sortingLayer = sortingLayer;
        this.orderInLayer = orderInLayer;
        this.color = color;
        this.flipX = flipX;
        this.flipY = flipY;
    }
}