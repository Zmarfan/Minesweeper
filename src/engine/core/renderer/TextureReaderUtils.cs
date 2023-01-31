using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.renderer; 

public static class TextureReaderUtils {
    public static unsafe Color[,] ReadSurfacePixels(SDL.SDL_Surface* surface) {
        Color[,] map = new Color[surface->w, surface->h];
        for (int x = 0; x < surface->w; x++) {
            for (int y = 0; y < surface->h; y++) {
                map[x, y] = ReadPixel(surface, x, y);
            }
        }
        
        return map;
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
}