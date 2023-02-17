using Worms.engine.core.renderer.textures;

namespace Worms.engine.core.assets; 

public record Assets(IEnumerable<AssetDeclaration> audioDeclarations, IEnumerable<AssetDeclaration> textureDeclarations, IEnumerable<AssetDeclaration> fontDeclarations) {
    public readonly IEnumerable<AssetDeclaration> audioDeclarations = audioDeclarations;
    public readonly IEnumerable<AssetDeclaration> textureDeclarations = textureDeclarations;
    public readonly IEnumerable<AssetDeclaration> fontDeclarations = fontDeclarations;
}