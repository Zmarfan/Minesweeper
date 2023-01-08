using Worms.engine.data;

namespace Worms.engine.game_object.components.texture_renderer; 

public class TextureRendererBuilder {
    private readonly string _textureSrc;
    private Color _color = Color.White();
    private bool _flipX;
    private bool _flipY;

    public TextureRendererBuilder(string textureSrc) {
        _textureSrc = textureSrc;
    }

    public static TextureRendererBuilder Builder(string textureSrc) {
        return new TextureRendererBuilder(textureSrc);
    }

    public TextureRenderer Build() {
        return new TextureRenderer(_textureSrc, _color, _flipX, _flipY);
    }
    
    public TextureRendererBuilder SetColor(Color color) {
        _color = color;
        return this;
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