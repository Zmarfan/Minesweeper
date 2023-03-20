using SDL2;
using GameEngine.engine.core.renderer.textures;

namespace GameEngine.engine.core.renderer.font; 

public class FontHandler {
    public readonly Dictionary<string, Font> fonts;

    public FontHandler(nint renderer, IEnumerable<AssetDeclaration> declarations) {
        if (SDL_ttf.TTF_Init() != 0) {
            throw new Exception($"Unable to load sdl ttf due to: {SDL_ttf.TTF_GetError()}");
        }

        fonts = declarations.ToDictionary(f => f.id, f => new Font(renderer, f.src));
    }

    public void Clean() {
        foreach (KeyValuePair<string, Font> entry in fonts) {
            SDL.SDL_DestroyTexture(entry.Value.textureAtlas);
        }
    }
}