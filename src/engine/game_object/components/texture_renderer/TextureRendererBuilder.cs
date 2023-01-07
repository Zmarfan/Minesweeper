namespace Worms.engine.game_object.components.texture_renderer; 

public class TextureRendererBuilder {
    private readonly string _textureSrc;
    private bool _flipX;
    private bool _flipY;

    public TextureRendererBuilder(string textureSrc) {
        _textureSrc = textureSrc;
    }

    public static TextureRendererBuilder Builder(string textureSrc) {
        return new TextureRendererBuilder(textureSrc);
    }

    public TextureRenderer Build() {
        return new TextureRenderer(_textureSrc, _flipX, _flipY);
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