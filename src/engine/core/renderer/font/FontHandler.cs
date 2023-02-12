using SDL2;

namespace Worms.engine.core.renderer.font; 

public class FontHandler {
    public readonly Dictionary<string, Font> fonts;

    public FontHandler(IntPtr renderer, IEnumerable<FontDefinition> fontDefinitions) {
        if (SDL_ttf.TTF_Init() != 0) {
            throw new Exception($"Unable to load sdl ttf due to: {SDL_ttf.TTF_GetError()}");
        }

        fonts = fontDefinitions.ToDictionary(f => f.name, f => new Font(renderer, f.src));
    }

    public void Clean() {
        foreach (KeyValuePair<string, Font> entry in fonts) {
            SDL.SDL_DestroyTexture(entry.Value.textureAtlas);
        }
    }
}