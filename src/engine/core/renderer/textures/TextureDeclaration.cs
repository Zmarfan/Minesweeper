namespace Worms.engine.core.renderer.textures; 

public readonly struct TextureDeclaration {
    public readonly string src;
    public readonly string id;

    public TextureDeclaration(string src, string id) {
        this.src = src;
        this.id = id;
    }
}