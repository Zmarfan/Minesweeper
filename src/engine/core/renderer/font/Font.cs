using SDL2;
using GameEngine.engine.core.renderer.textures;
using GameEngine.engine.data;

namespace GameEngine.engine.core.renderer.font; 

internal class Font {
    public const int ATLAS_SIZE = 2048;
    public const int FONT_SIZE = 55;
    private const char START_CHAR = ' ';
    private const char END_CHAR = 'ÿ';

    private static readonly SDL.SDL_Color DEFAULT_GLYPH_COLOR = new() { r = byte.MaxValue, g = byte.MaxValue, b = byte.MaxValue, a = byte.MaxValue };
    private static readonly List<char> SUPPORTED_CHARACTERS = new(); 
    
    static Font() {
        for (char c = START_CHAR; c <= END_CHAR; c++) {
            SUPPORTED_CHARACTERS.Add(c);
        }
    }
    
    private readonly nint _ttfFont;
    public readonly Dictionary<char, CharacterInfo> characters = new();
    public readonly int maxCharHeight;
    public readonly nint textureAtlas;

    public Font(nint renderer, string fontSrc) {
        _ttfFont = SDL_ttf.TTF_OpenFont(fontSrc, FONT_SIZE);
        if (_ttfFont == nint.Zero) {
            throw new ArgumentException($"Unable to load font: {fontSrc} due to: {SDL_ttf.TTF_GetError()}");
        }

        textureAtlas = CreateTextureAtlas(renderer);
        CalculateSupportedCharacterKerning();
        maxCharHeight = characters.Values.MaxBy(c => c.dimension.y)!.dimension.y;
    }

    private unsafe nint CreateTextureAtlas(nint renderer) {
        SDL.SDL_Surface* atlas = (SDL.SDL_Surface*)SDL.SDL_CreateRGBSurfaceWithFormat(0, ATLAS_SIZE, ATLAS_SIZE, 32, SDL.SDL_PIXELFORMAT_ABGR8888);
        
        SDL.SDL_Rect destination = new() { x = 0, y = 0 };
        foreach (char c in SUPPORTED_CHARACTERS) {
            TryAddGlyphToAtlas(c, ref atlas, ref destination);
        }

        return SurfaceReadWriteUtils.SurfaceToTexture(renderer, atlas);
    }

    private unsafe void TryAddGlyphToAtlas(char c, ref SDL.SDL_Surface* atlas, ref SDL.SDL_Rect destination) {
        if (SDL_ttf.TTF_GlyphIsProvided32(_ttfFont, c) == 0) {
            return;
        }
            
        SDL.SDL_Surface* glyph = (SDL.SDL_Surface*)SDL_ttf.TTF_RenderGlyph32_Solid(_ttfFont, c, DEFAULT_GLYPH_COLOR);
        if ((nint)glyph == nint.Zero) {
            return;
        }
        SurfaceReadWriteUtils.FormatSurface(ref glyph);
        if (c != ' ' && !SurfaceHasContent(glyph)) {
            return;
        }
        
        SDL_ttf.TTF_SizeUTF8(_ttfFont, c.ToString(), out destination.w, out destination.h);

        destination = MoveDownRowIfNeeded(destination);
        characters.Add(c, new CharacterInfo(destination));
        if (SDL.SDL_BlitSurface((nint)glyph, nint.Zero, (nint)atlas, ref destination) != 0) {
            throw new Exception($"Unable to blit char due to: {SDL.SDL_GetError()})");
        }
        destination.x += destination.w;
        SDL.SDL_FreeSurface((nint)glyph);
    }

    private static SDL.SDL_Rect MoveDownRowIfNeeded(SDL.SDL_Rect destination) {
        if (destination.x + destination.w < ATLAS_SIZE) {
            return destination;
        }
        destination.x = 0;
        destination.y += destination.h + 1;

        if (destination.y + destination.h >= ATLAS_SIZE) {
            throw new Exception("Not all characters in font can fit in texture atlas!");
        }

        return destination;
    }

    private static unsafe bool SurfaceHasContent(SDL.SDL_Surface* surface) {
        return SurfaceReadWriteUtils.ReadSurfacePixels(surface).Cast<Color>().Any(c => c.a > 0);
    }

    private void CalculateSupportedCharacterKerning() {
        foreach ((char character, CharacterInfo info) in characters) {
            foreach ((char previous, CharacterInfo _) in characters) {
                int kerning = SDL_ttf.TTF_GetFontKerningSizeGlyphs32(_ttfFont, previous, character);
                info.kerningByCharacter.Add(previous, kerning);
            }
        }
    }
}