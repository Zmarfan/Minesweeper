using SDL2;
using Worms.engine.core.game_object_handler;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.logger;
using Worms.engine.scene;

namespace Worms.engine.core.renderer; 

public static class TextureRendererHandler {
    private const string ACCEPTED_PIXEL_FORMAT = "SDL_PIXELFORMAT_ABGR8888";
    public const string DEFAULT_SORTING_LAYER = "Default";

    private static IntPtr _renderer;
    private static SceneData _sceneData = null!;
    private static readonly Dictionary<string, StoredTexture> LOADED_TEXTURES = new();
    private static readonly List<string> SORT_LAYERS = new() { DEFAULT_SORTING_LAYER };

    public static void Init(IntPtr renderer, GameSettings settings, SceneData sceneData) {
        _renderer = renderer;
        _sceneData = sceneData;
        SORT_LAYERS.AddRange(settings.sortLayers);
    }

    public static unsafe void LoadImage(string textureId, string textureSrc, out SDL.SDL_Surface* surface, out Color[,] pixels) {
        if (LOADED_TEXTURES.TryGetValue(textureId, out StoredTexture? storedTexture)) {
            pixels = storedTexture.pixels;
            surface = storedTexture.surface;
            return;
        }

        surface = LoadSurfaceWithCorrectFormat(textureSrc);
        pixels = TextureReaderUtils.ReadSurfacePixels(surface);
    }

    public static void RenderTextures(Dictionary<GameObject, TrackObject> objects) {
        IEnumerable<TextureRenderer> textureRenderers = objects
            .Values
            .Where(obj => obj.isActive)
            .SelectMany(obj => obj.TextureRenderers)
            .Where(tr => tr.IsActive)
            .OrderByDescending(tr => SORT_LAYERS.IndexOf(tr.sortingLayer))
            .ThenByDescending(tr => tr.orderInLayer);
        
        foreach (TextureRenderer tr in textureRenderers) {
            try {
                TransformationMatrix matrix = objects[tr.gameObject].isWorld ? _sceneData.camera.WorldToScreenMatrix : _sceneData.camera.UiToScreenMatrix;
                RenderTexture(tr, matrix);
            }
            catch (ArgumentException e) {
                Logger.Error(e);
            }
        }
    }

    private static unsafe void RenderTexture(TextureRenderer tr, TransformationMatrix matrix) {
        StoredTexture texture = GetTexture(tr);

        SDL.SDL_Rect srcRect = tr.texture.GetSrcRect(texture);
        SDL.SDL_FRect destRect = CalculateTextureDrawPosition(tr, texture.surface, matrix);
        SDL.SDL_SetTextureColorMod(texture.texture, tr.color.Rbyte, tr.color.Gbyte, tr.color.Bbyte);
        SDL.SDL_RenderCopyExF(_renderer, texture.texture, ref srcRect, ref destRect, tr.Transform.Rotation.Degree, IntPtr.Zero, GetTextureFlipSettings(tr));
    }
    
    private static unsafe StoredTexture GetTexture(TextureRenderer tr) {
        if (!LOADED_TEXTURES.TryGetValue(tr.texture.textureId, out StoredTexture? texture)) {
            IntPtr texturePtr = SDL.SDL_CreateTextureFromSurface(_renderer, (IntPtr)tr.texture.surface);
            if (texturePtr == IntPtr.Zero) {
                throw new ArgumentException($"Unable to load texture: {tr.texture} due to: {SDL.SDL_GetError()}");
            }
            texture = new StoredTexture(tr.texture.surface, texturePtr, tr.texture.pixels);
            LOADED_TEXTURES.Add(tr.texture.textureId, texture);
        }

        return texture;
    }
    
    private static unsafe SDL.SDL_FRect CalculateTextureDrawPosition(TextureRenderer tr, SDL.SDL_Surface* surface, TransformationMatrix matrix) {
        Vector2 screenPosition = CalculateTextureScreenPosition(tr, surface, matrix);
        Vector2 textureDimensions = CalculateTextureDimensions(tr, surface, matrix);
        SDL.SDL_FRect rect = new() {
            x = screenPosition.x,
            y = screenPosition.y,
            w = textureDimensions.x,
            h = textureDimensions.y
        };
        return rect;
    }

    private static unsafe Vector2 CalculateTextureScreenPosition(TextureRenderer tr, SDL.SDL_Surface* surface, TransformationMatrix matrix) {
        return matrix.ConvertPoint(tr.Transform.Position) - CalculateTextureDimensions(tr, surface, matrix) / 2f;
    }

    private static unsafe Vector2 CalculateTextureDimensions(TextureRenderer tr, SDL.SDL_Surface* surface, TransformationMatrix matrix) {
        return matrix.ConvertVector(
            new Vector2(surface->w * tr.Transform.Scale.x, surface->h * tr.Transform.Scale.y * -1) * tr.texture.textureScale
        );
    }
    
    private static SDL.SDL_RendererFlip GetTextureFlipSettings(TextureRenderer tr) {
        SDL.SDL_RendererFlip flip = SDL.SDL_RendererFlip.SDL_FLIP_NONE;
        if (tr.flipX) {
            flip |= SDL.SDL_RendererFlip.SDL_FLIP_HORIZONTAL;
        }
        if (tr.flipY) {
            flip |= SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL;
        }
        return flip;
    }
    
    private static unsafe SDL.SDL_Surface* LoadSurfaceWithCorrectFormat(string textureSrc) {
        SDL.SDL_Surface* surface = (SDL.SDL_Surface*)SDL_image.IMG_Load(textureSrc);
        if (SDL.SDL_GetPixelFormatName(((SDL.SDL_PixelFormat*)surface->format)->format) != ACCEPTED_PIXEL_FORMAT) {
            SDL.SDL_Surface* convertedSurface = (SDL.SDL_Surface*)SDL.SDL_ConvertSurfaceFormat((nint)surface, SDL.SDL_PIXELFORMAT_ABGR8888, 0);
            SDL.SDL_FreeSurface((nint)surface);
            surface = convertedSurface;
        }

        return surface;
    }

    public static unsafe void Clean() {
        LOADED_TEXTURES.Values.ToList().ForEach(texture => {
            SDL.SDL_FreeSurface((nint)texture.surface);
            SDL.SDL_DestroyTexture(texture.texture);
        });
    }
}