using SDL2;
using Worms.engine.data;
using Worms.engine.game_object.components.texture_renderer;

namespace Worms.engine.core.renderer; 

public class TextureRendererHandler {
    public const string DEFAULT_SORTING_LAYER = "Default";

    private readonly IntPtr _renderer;
    private readonly GameSettings _settings;
    private readonly Dictionary<string, StoredTexture> _loadedTextures = new();
    private readonly List<string> _sortLayers = new() { DEFAULT_SORTING_LAYER };

    public TextureRendererHandler(IntPtr renderer, GameSettings settings) {
        _renderer = renderer;
        _settings = settings;
        _sortLayers.AddRange(settings.sortLayers);
    }

    public void RenderTextures(IEnumerable<TextureRenderer> allActiveTextureRenderers) {
        foreach (TextureRenderer tr in allActiveTextureRenderers.OrderByDescending(tr => _sortLayers.IndexOf(tr.sortingLayer)).ThenByDescending(tr => tr.orderInLayer)) {
            if (tr.IsActive) {
                try {
                    RenderTexture(tr);
                }
                catch (ArgumentException e) {
                    Console.WriteLine(e);
                }
            }
        }
    }

    private unsafe void RenderTexture(TextureRenderer tr) {
        StoredTexture texture = GetTexture(tr);

        SDL.SDL_FRect destRect = WorldToScreenCalculator.CalculateTextureDrawPosition(tr.Transform, texture.surface, _settings);
        SDL.SDL_SetTextureColorMod(texture.texture, tr.color.Rbyte, tr.color.Gbyte, tr.color.Bbyte);
        SDL.SDL_Rect srcRect = tr.texture.GetSrcRect(texture);
        SDL.SDL_RenderCopyExF(_renderer, texture.texture, ref srcRect, ref destRect, tr.Transform.WorldRotation.Degree, IntPtr.Zero, GetTextureFlipSettings(tr));
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