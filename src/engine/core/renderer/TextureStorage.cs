using SDL2;
using Worms.engine.data;
using Worms.engine.game_object.components.texture_renderer;

namespace Worms.engine.core.renderer; 

public static class TextureStorage {
    private static readonly Dictionary<string, StoredTexture> LOADED_TEXTURES = new();
    
    public static unsafe void LoadImageFromFile(string textureSrc, out SDL.SDL_Surface* surface, out Color[,] pixels) {
        if (LOADED_TEXTURES.TryGetValue(textureSrc, out StoredTexture? storedTexture)) {
            pixels = storedTexture.pixels;
            surface = storedTexture.surface;
            return;
        }

        surface = SurfaceReadWriteUtils.LoadSurfaceFromFile(textureSrc);
        pixels = SurfaceReadWriteUtils.ReadSurfacePixels(surface);
    }

    public static void RemoveLoadedTexture(string textureId) {
        if (LOADED_TEXTURES.TryGetValue(textureId, out StoredTexture? storedTexture)) {
            SDL.SDL_DestroyTexture(storedTexture.texture);
        }
        LOADED_TEXTURES.Remove(textureId);
    }
    
    public static unsafe StoredTexture GetAndCacheTexture(IntPtr renderer, Texture texture) {
        if (!LOADED_TEXTURES.TryGetValue(texture.textureId, out StoredTexture? storedTexture)) {
            IntPtr texturePtr = SurfaceReadWriteUtils.SurfaceToTexture(renderer, (IntPtr)texture.surface);
            storedTexture = new StoredTexture(texture.surface, texturePtr, texture.texturePixels);
            LOADED_TEXTURES.Add(texture.textureId, storedTexture);
        }

        return storedTexture;
    }
    
    public static unsafe void Clean() {
        LOADED_TEXTURES.Values.ToList().ForEach(texture => {
            SDL.SDL_FreeSurface((nint)texture.surface);
            SDL.SDL_DestroyTexture(texture.texture);
        });
    }
}