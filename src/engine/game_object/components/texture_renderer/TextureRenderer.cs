using SDL2;
using Worms.engine.core;

namespace Worms.engine.game_object.components.texture_renderer; 

public class TextureRenderer : Component {
    public string textureSrc;
    public bool flipX;
    public bool flipY;
    
    public TextureRenderer(string textureSrc, bool flipX, bool flipY) {
        this.textureSrc = textureSrc;
        this.flipX = flipX;
        this.flipY = flipY;
    }
}