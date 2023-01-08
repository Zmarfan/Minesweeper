using SDL2;

namespace Worms.engine.core.renderer; 

public class StoredTexture {
    public readonly unsafe SDL.SDL_Surface* surface;
    public readonly IntPtr texture;

    public unsafe StoredTexture(SDL.SDL_Surface* surface, nint texture) {
        this.surface = surface;
        this.texture = texture;
    }
}