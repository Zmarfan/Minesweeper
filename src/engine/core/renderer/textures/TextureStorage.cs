using SDL2;
using Worms.engine.data;
using Worms.engine.game_object.components.rendering.texture_renderer;

namespace Worms.engine.core.renderer.textures; 

public class TextureStorage {
    private static TextureStorage _self = null!;

    private readonly IntPtr _renderer;
    private readonly Dictionary<string, StoredTexture> _loadedTextures = new();
    
    private TextureStorage(IntPtr renderer, IEnumerable<TextureDeclaration> textureDeclarations) {
        _renderer = renderer;
        foreach (TextureDeclaration declaration in textureDeclarations) {
            PreLoadTextureDeclaration(declaration);
        }
    }

    public static TextureStorage Init(IntPtr renderer, IEnumerable<TextureDeclaration> textureDeclarations) {
        if (_self != null) {
            throw new Exception("There can only be one TextureStorage at a time!");
        }

        _self = new TextureStorage(renderer, textureDeclarations);
        return _self;
    }
    
    public static StoredTexture GetStoredTexture(string textureId) {
        try {
            return _self._loadedTextures[textureId];
        }
        catch (KeyNotFoundException e) {
            throw new ArgumentException($"Unable to fetch loaded texture for: {textureId}. This is most likely due to it not being defined in the game settings!", e);
        }
    }

    public static unsafe void LoadTextureFromPixels(Color[,] pixels, out string textureId) {
        textureId = Guid.NewGuid().ToString();
        SDL.SDL_Surface* surface = SurfaceReadWriteUtils.WriteSurfacePixels(pixels);
        _self._loadedTextures.Add(textureId, new StoredTexture(
            surface,
            SurfaceReadWriteUtils.SurfaceToTexture(_self._renderer, (IntPtr)surface),
            pixels,
            false
        ));
    }
    
    public static unsafe void AlterTexture(
        string currentTextureId,
        Color[,] oldPixels,
        Color[,] pixels,
        out string newTextureId,
        out StoredTexture storedTexture
    ) {
        newTextureId = Guid.NewGuid().ToString();
        SDL.SDL_Surface* surface;
        if (_self._loadedTextures[currentTextureId].fromFile) {
            surface = SurfaceReadWriteUtils.WriteSurfacePixels(pixels);
        }
        else {
            StoredTexture oldStoredTexture = _self._loadedTextures[currentTextureId];
            SDL.SDL_DestroyTexture(oldStoredTexture.texture);
            SurfaceReadWriteUtils.AlterSurfacePixels(oldStoredTexture.surface, oldPixels, pixels);
            surface = oldStoredTexture.surface;
            _self._loadedTextures.Remove(currentTextureId);
        }
        _self._loadedTextures.Add(newTextureId, new StoredTexture(
            surface, 
            SurfaceReadWriteUtils.SurfaceToTexture(_self._renderer, (IntPtr)surface),
            pixels,
            false
        ));
        storedTexture = _self._loadedTextures[newTextureId];
    }

    private unsafe void PreLoadTextureDeclaration(TextureDeclaration declaration) {
        SDL.SDL_Surface* surface = SurfaceReadWriteUtils.LoadSurfaceFromFile(declaration.src);
        _loadedTextures.Add(declaration.id, new StoredTexture(
            surface,
            SurfaceReadWriteUtils.SurfaceToTexture(_renderer, (IntPtr)surface),
            SurfaceReadWriteUtils.ReadSurfacePixels(surface),
            true
        ));
    }
    
    public unsafe void Clean() {
        _loadedTextures.Values.ToList().ForEach(texture => {
            SDL.SDL_FreeSurface((nint)texture.surface);
            SDL.SDL_DestroyTexture(texture.texture);
        });
    }
}