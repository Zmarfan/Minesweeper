using SDL2;
using Worms.engine.core.game_object_handler;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.logger;
using Worms.engine.scene;

namespace Worms.engine.core.renderer; 

public class TextureRendererHandler {
    public const string DEFAULT_SORTING_LAYER = "Default";

    private readonly IntPtr _renderer;
    private readonly SceneData _sceneData;
    private readonly Dictionary<string, StoredTexture> _loadedTextures = new();
    private readonly List<string> _sortLayers = new() { DEFAULT_SORTING_LAYER };

    public TextureRendererHandler(IntPtr renderer, GameSettings settings, SceneData sceneData) {
        _renderer = renderer;
        _sceneData = sceneData;
        _sortLayers.AddRange(settings.sortLayers);
    }

    public void RenderTextures(Dictionary<GameObject, TrackObject> objects) {
        IEnumerable<TextureRenderer> textureRenderers = objects
            .Values
            .Where(obj => obj.isActive)
            .SelectMany(obj => obj.textureRenderers)
            .Where(tr => tr.IsActive)
            .OrderByDescending(tr => _sortLayers.IndexOf(tr.sortingLayer))
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

    private unsafe void RenderTexture(TextureRenderer tr, TransformationMatrix matrix) {
        StoredTexture texture = GetTexture(tr);

        SDL.SDL_Rect srcRect = tr.texture.GetSrcRect(texture);
        SDL.SDL_FRect destRect = CalculateTextureDrawPosition(tr, texture.surface, matrix);
        SDL.SDL_SetTextureColorMod(texture.texture, tr.color.Rbyte, tr.color.Gbyte, tr.color.Bbyte);
        SDL.SDL_RenderCopyExF(_renderer, texture.texture, ref srcRect, ref destRect, tr.Transform.Rotation.Degree, IntPtr.Zero, GetTextureFlipSettings(tr));
    }
    
    private unsafe StoredTexture GetTexture(TextureRenderer tr) {
        if (!_loadedTextures.TryGetValue(tr.texture.textureSrc, out StoredTexture? texture)) {
            IntPtr texturePtr = SDL_image.IMG_LoadTexture(_renderer, tr.texture.textureSrc);
            if (texturePtr == IntPtr.Zero) {
                throw new ArgumentException($"Unable to load texture: {tr.texture} due to: {SDL_image.IMG_GetError()}");
            }
            SDL.SDL_Surface * surface = (SDL.SDL_Surface *)SDL_image.IMG_Load(tr.texture.textureSrc);
            texture = new StoredTexture(surface, texturePtr);
            _loadedTextures.Add(tr.texture.textureSrc, texture);
        }

        return texture;
    }
    
    private unsafe SDL.SDL_FRect CalculateTextureDrawPosition(TextureRenderer tr, SDL.SDL_Surface* surface, TransformationMatrix matrix) {
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

    private unsafe Vector2 CalculateTextureScreenPosition(TextureRenderer tr, SDL.SDL_Surface* surface, TransformationMatrix matrix) {
        return matrix.ConvertPoint(tr.Transform.Position) - CalculateTextureDimensions(tr, surface, matrix) / 2f;
    }

    private unsafe Vector2 CalculateTextureDimensions(TextureRenderer tr, SDL.SDL_Surface* surface, TransformationMatrix matrix) {
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

    public unsafe void Clean() {
        _loadedTextures.Values.ToList().ForEach(texture => {
            SDL.SDL_FreeSurface((nint)texture.surface);
            SDL.SDL_DestroyTexture(texture.texture);
        });
    }
}