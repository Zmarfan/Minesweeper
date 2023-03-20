using SDL2;
using GameEngine.engine.data;

namespace GameEngine.engine.core.renderer.textures; 

internal static class SurfaceReadWriteUtils {
    private const string ACCEPTED_PIXEL_FORMAT = "SDL_PIXELFORMAT_ABGR8888";
    
    public static unsafe nint SurfaceToTexture(nint renderer, SDL.SDL_Surface* surface) {
        nint texture = SDL.SDL_CreateTextureFromSurface(renderer, (nint)surface);
        if (texture == nint.Zero) {
            throw new ArgumentException($"Unable to load surface due to: {SDL.SDL_GetError()}");
        }

        if (SDL.SDL_SetTextureBlendMode(texture, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND) != 0) {
            throw new Exception($"Unable to set texture blend mode, used for alpha mod, due to: {SDL.SDL_GetError()}");
        }

        return texture;
    }
    
    public static unsafe SDL.SDL_Surface* LoadSurfaceFromFile(string textureSrc) {
        SDL.SDL_Surface* surface = (SDL.SDL_Surface*)SDL_image.IMG_Load(textureSrc);
        FormatSurface(ref surface);
        return surface;
    }

    public static unsafe void FormatSurface(ref SDL.SDL_Surface* surface) {
        if (SDL.SDL_GetPixelFormatName(((SDL.SDL_PixelFormat*)surface->format)->format) != ACCEPTED_PIXEL_FORMAT) {
            SDL.SDL_Surface* convertedSurface = (SDL.SDL_Surface*)SDL.SDL_ConvertSurfaceFormat((nint)surface, SDL.SDL_PIXELFORMAT_ABGR8888, 0);
            SDL.SDL_FreeSurface((nint)surface);
            surface = convertedSurface;
        }
    }
    
    public static unsafe Color[,] ReadSurfacePixels(SDL.SDL_Surface* surface) {
        Color[,] map = new Color[surface->w, surface->h];
        for (int x = 0; x < surface->w; x++) {
            for (int y = 0; y < surface->h; y++) {
                map[x, y] = ReadPixel(surface, x, y);
            }
        }
        
        return map;
    }
    
    public static unsafe SDL.SDL_Surface* WriteSurfacePixels(Color[,] pixels) {
        SDL.SDL_Surface* surface = (SDL.SDL_Surface*)SDL.SDL_CreateRGBSurfaceWithFormat(
            0,
            pixels.GetLength(0),
            pixels.GetLength(1),
            32,
            SDL.SDL_PIXELFORMAT_ABGR8888
        );
        for (int x = 0; x < surface->w; x++) {
            for (int y = 0; y < surface->h; y++) {
                WritePixel(surface, pixels[x, y], x, y);
            }
        }

        return surface;
    }
    
    public static unsafe void AlterSurfacePixels(SDL.SDL_Surface* surface, Color[,] oldPixels, Color[,] pixels) {
        for (int x = 0; x < surface->w; x++) {
            for (int y = 0; y < surface->h; y++) {
                if (oldPixels[x, y] != pixels[x, y]) {
                    WritePixel(surface, pixels[x, y], x, y);
                }
            }
        }
    }

    private static unsafe Color ReadPixel(SDL.SDL_Surface* surface, int x, int y) {
        byte bytesPerPixel = ((SDL.SDL_PixelFormat*)surface->format)->BytesPerPixel;
        uint pixelData = GetPixelData(surface, bytesPerPixel, surface->w * y + x);
        SDL.SDL_GetRGBA(pixelData, surface->format, out byte r, out byte g, out byte b, out byte a);
        return new Color(r, g, b, a);
    }

    private static unsafe uint GetPixelData(SDL.SDL_Surface* surface, byte bytesPerPixel, int index) {
        return bytesPerPixel switch {
            1 => ((byte*)surface->pixels)[index],
            2 => ((ushort*)surface->pixels)[index],
            4 => ((uint*)surface->pixels)[index],
            _ => throw new Exception($"Error when trying to read pixel data! bytesPerPixel: {bytesPerPixel}")
        };
    }
    
    private static unsafe void WritePixel(SDL.SDL_Surface* surface, Color color, int x, int y) {
        uint pixelData = SDL.SDL_MapRGBA(surface->format, color.Rbyte, color.Gbyte, color.Bbyte, color.Abyte);
        byte bytesPerPixel = ((SDL.SDL_PixelFormat*)surface->format)->BytesPerPixel;
        WritePixelData(surface, bytesPerPixel, pixelData, surface->w * y + x);
    }
    
    private static unsafe void WritePixelData(SDL.SDL_Surface* surface, byte bytesPerPixel, uint pixelData, int index) {
        switch (bytesPerPixel) {
            case 1:
                ((byte*)surface->pixels)[index] = (byte)pixelData;
                break;
            case 2:
                ((ushort*)surface->pixels)[index] = (ushort)pixelData;
                break;
            case 4:
                ((uint*)surface->pixels)[index] = pixelData;
                break;
            default:
                throw new Exception($"Error when trying to write pixel data! bytesPerPixel: {bytesPerPixel}");
        }
    }
}