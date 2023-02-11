using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.renderer.font; 

public class Font {
    private const int ATLAS_SIZE = 1024;
    private const int FONT_SIZE = 55;
    private const char START_CHAR = ' ';
    private const char END_CHAR = 'ÿ';

    private static readonly SDL.SDL_Color DEFAULT_GLYPH_COLOR = new() { r = byte.MaxValue, g = byte.MaxValue, b = byte.MaxValue, a = byte.MaxValue };
    private static readonly List<uint> SUPPORTED_CHARACTERS = new(); 
    
    static Font() {
        for (uint c = START_CHAR; c <= END_CHAR; c++) {
            SUPPORTED_CHARACTERS.Add(c);
        }
    }
    
    private readonly IntPtr _ttfFont;
    public readonly Dictionary<char, CharacterInfo> characters = new();
    public readonly IntPtr textureAtlas;

    public unsafe Font(IntPtr renderer, string fontSrc, SDL.SDL_Surface* missingCharacter) {
        _ttfFont = SDL_ttf.TTF_OpenFont(fontSrc, FONT_SIZE);
        if (_ttfFont == IntPtr.Zero) {
            throw new ArgumentException($"Unable to load font: {fontSrc} due to: {SDL_ttf.TTF_GetError()}");
        }

        textureAtlas = CreateTextureAtlas(renderer, missingCharacter);
        CalculateSupportedCharacterKerning();
    }

    private unsafe nint CreateTextureAtlas(IntPtr renderer, SDL.SDL_Surface* missingCharacter) {
        SDL.SDL_Surface* atlas = (SDL.SDL_Surface*)SDL.SDL_CreateRGBSurfaceWithFormat(0, ATLAS_SIZE, ATLAS_SIZE, 32, SDL.SDL_PIXELFORMAT_ABGR8888);
        
        SDL.SDL_Rect destination = new() { x = 0, y = 0 };
        foreach (uint c in SUPPORTED_CHARACTERS) {
            TryAddGlyphToAtlas(c, ref atlas, ref destination);
        }

        destination.w = missingCharacter->w;
        destination.h = missingCharacter->h;
        AddGlyphToAtlas(ref destination, ref missingCharacter, ref atlas);
        return SurfaceReadWriteUtils.SurfaceToTexture(renderer, (IntPtr)atlas);
    }

    private unsafe void TryAddGlyphToAtlas(uint c, ref SDL.SDL_Surface* atlas, ref SDL.SDL_Rect destination) {
        if (SDL_ttf.TTF_GlyphIsProvided32(_ttfFont, c) == 0) {
            return;
        }
            
        SDL.SDL_Surface* glyph = (SDL.SDL_Surface*)SDL_ttf.TTF_RenderGlyph32_Blended(_ttfFont, c, DEFAULT_GLYPH_COLOR);
        if ((IntPtr)glyph == IntPtr.Zero) {
            return;
        }
        SurfaceReadWriteUtils.FormatSurface(ref glyph);
        if (!SurfaceHasContent(glyph)) {
            return;
        }
        
        SDL_ttf.TTF_SizeUTF8(_ttfFont, ((char)c).ToString(), out destination.w, out destination.h);

        AddGlyphToAtlas(ref destination, ref glyph, ref atlas);
        SDL.SDL_FreeSurface((IntPtr)glyph);
        
        characters.Add((char)c, new CharacterInfo((char)c, destination));
    }

    private static unsafe void AddGlyphToAtlas(ref SDL.SDL_Rect destination, ref SDL.SDL_Surface* glyph, ref SDL.SDL_Surface* atlas) {
        if (destination.x + destination.w >= ATLAS_SIZE) {
            destination.x = 0;
            destination.y += destination.h + 1;

            if (destination.y + destination.h >= ATLAS_SIZE) {
                throw new Exception($"Not all characters in font can fit in texture atlas!");
            }
        }

        if (SDL.SDL_BlitSurface((IntPtr)glyph, IntPtr.Zero, (IntPtr)atlas, ref destination) != 0) {
            throw new Exception($"Unable to blit char due to: {SDL.SDL_GetError()})");
        }
        
        destination.x += destination.w;
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