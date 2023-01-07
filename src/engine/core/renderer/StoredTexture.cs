using SDL2;

namespace Worms.engine.core.renderer; 

public class StoredTexture {
    public unsafe SDL.SDL_Surface* surface;
    public IntPtr texture;

    public unsafe StoredTexture(SDL.SDL_Surface* surface, nint texture) {
        this.surface = surface;
        this.texture = texture;
    }
}