using Worms.engine.data;

namespace Worms.engine.game_object.components.texture_renderer; 

public class TextureRenderer : Component {
    private static readonly string ROOT_DIRECTORY = Directory.GetCurrentDirectory();
    
    public string textureSrc;
    public Color color;
    public bool flipX;
    public bool flipY;
    
    public TextureRenderer(string textureSrc, Color color, bool flipX, bool flipY) {
        this.textureSrc = $"{ROOT_DIRECTORY}\\{textureSrc}";
        this.color = color;
        this.flipX = flipX;
        this.flipY = flipY;
    }
}