using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.renderer; 

public class StoredTexture {
    public readonly unsafe SDL.SDL_Surface* surface;
    public readonly IntPtr texture;
    public readonly Color[,] pixels;

    public unsafe StoredTexture(SDL.SDL_Surface* surface, nint texture, Color[,] pixels) {
        this.surface = surface;
        this.texture = texture;
        this.pixels = pixels;
    }
}