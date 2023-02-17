namespace Worms.engine.core.renderer.textures; 

public readonly struct AssetDeclaration {
    public readonly string src;
    public readonly string id;

    public AssetDeclaration(string src, string id) {
        this.src = src;
        this.id = id;
    }
}