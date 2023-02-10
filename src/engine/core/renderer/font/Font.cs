using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.renderer.font; 

public class Font {
    private const int ATLAS_SIZE = 1024;
    private const int FONT_SIZE = 85;
    private const char START_CHAR = ' ';
    private const char END_CHAR = 'ÿ';

    private static readonly SDL.SDL_Color DEFAULT_GLYPH_COLOR = new() { r = byte.MaxValue, g = byte.MaxValue, b = byte.MaxValue, a = byte.MaxValue };
    
    private readonly IntPtr _ttfFont;
    private readonly Dictionary<char, SDL.SDL_Rect> _characterRects = new();
    public readonly IntPtr _textureAtlas;

    public unsafe Font(IntPtr renderer, string fontSrc, SDL.SDL_Surface* missingCharacter) {
        _ttfFont = SDL_ttf.TTF_OpenFont(fontSrc, FONT_SIZE);
        if (_ttfFont == IntPtr.Zero) {
            throw new ArgumentException($"Unable to load font: {fontSrc} due to: {SDL_ttf.TTF_GetError()}");
        }

        _textureAtlas = CreateTextureAtlas(renderer, missingCharacter);
    }

    private unsafe nint CreateTextureAtlas(IntPtr renderer, SDL.SDL_Surface* missingCharacter) {
        SDL.SDL_Surface* atlas = (SDL.SDL_Surface*)SDL.SDL_CreateRGBSurfaceWithFormat(0, ATLAS_SIZE, ATLAS_SIZE, 32, SDL.SDL_PIXELFORMAT_ABGR8888);
        
        SDL.SDL_Rect destination = new() { x = 0, y = 0 };

        for (uint c = START_CHAR; c <= END_CHAR; c++) {
            AddGlyphToAtlas(c, ref atlas, ref destination);
        }

        destination.w = missingCharacter->w;
        destination.h = missingCharacter->h;
        SDL.SDL_BlitSurface((IntPtr)missingCharacter, IntPtr.Zero, (IntPtr)atlas, ref destination);
        return SurfaceReadWriteUtils.SurfaceToTexture(renderer, (IntPtr)atlas);
    }

    private unsafe void AddGlyphToAtlas(uint c, ref SDL.SDL_Surface* atlas, ref SDL.SDL_Rect destination) {
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

        if (destination.x + destination.w >= ATLAS_SIZE) {
            destination.x = 0;
            destination.y += destination.h + 1;

            if (destination.y + destination.h >= ATLAS_SIZE) {
                throw new Exception($"Not all characters in font can fit in texture atlas! Failed on char: {(char)c}");
            }
        }

        if (SDL.SDL_BlitSurface((IntPtr)glyph, IntPtr.Zero, (IntPtr)atlas, ref destination) != 0) {
            throw new Exception($"Unable to blit char: {(char)c} due to: {SDL.SDL_GetError()})");
        }
        SDL.SDL_FreeSurface((IntPtr)glyph);
        _characterRects.Add((char)c, destination);
        destination.x += destination.w;
    }

    private unsafe bool SurfaceHasContent(SDL.SDL_Surface* surface) {
        return SurfaceReadWriteUtils.ReadSurfacePixels(surface).Cast<Color>().Any(c => c.a > 0);
    }
}