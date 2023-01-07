using SDL2;
using Worms.engine.core;

namespace Worms.engine.game_object.components.texture_renderer; 

public class TextureRenderer : Component {
    private static readonly string ROOT_DIRECTORY = Directory.GetCurrentDirectory();
    
    public string textureSrc;
    public bool flipX;
    public bool flipY;
    
    public TextureRenderer(string textureSrc, bool flipX, bool flipY) {
        this.textureSrc = $"{ROOT_DIRECTORY}\\{textureSrc}";
        this.flipX = flipX;
        this.flipY = flipY;
    }
}