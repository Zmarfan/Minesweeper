using Worms.engine.core.renderer.textures;

namespace Worms.engine.core.assets; 

public class AssetsBuilder {
    private readonly List<AssetDeclaration> _audioDeclarations = new();
    private readonly List<AssetDeclaration> _textureDeclarations = new();
    private readonly List<AssetDeclaration> _fontDeclarations = new();

    public static AssetsBuilder Builder() {
        return new AssetsBuilder();
    }

    public Assets Build() {
        return new Assets(_audioDeclarations, _textureDeclarations, _fontDeclarations);
    }

    public AssetsBuilder AddAudios(IEnumerable<AssetDeclaration> declarations) {
        _audioDeclarations.AddRange(declarations);
        return this;
    }
    
    public AssetsBuilder AddTextures(IEnumerable<AssetDeclaration> declarations) {
        _textureDeclarations.AddRange(declarations);
        return this;
    }
    
    public AssetsBuilder AddFonts(IEnumerable<AssetDeclaration> declarations) {
        _fontDeclarations.AddRange(declarations);
        return this;
    }
}