using SDL2;

namespace Worms.engine.core.renderer.font; 

public class FontHandler {
    public static readonly string MISSING_CHAR_SRC = $"{Directory.GetCurrentDirectory()}\\src\\assets\\font\\missing_character.png";

    public readonly Dictionary<string, Font> fonts;

    public unsafe FontHandler(IntPtr renderer, IEnumerable<FontDefinition> fontDefinitions) {
        if (SDL_ttf.TTF_Init() != 0) {
            throw new Exception($"Unable to load sdl ttf due to: {SDL_ttf.TTF_GetError()}");
        }

        SDL.SDL_Surface* missingCharacter = SurfaceReadWriteUtils.LoadSurfaceFromFile(MISSING_CHAR_SRC);
        fonts = fontDefinitions.ToDictionary(f => f.name, f => new Font(renderer, f.src, missingCharacter));
        SDL.SDL_FreeSurface((IntPtr)missingCharacter);
    }

    public void Clean() {
        foreach (KeyValuePair<string, Font> entry in fonts) {
            SDL.SDL_DestroyTexture(entry.Value.textureAtlas);
        }
    }
}