using SDL2;
using Worms.engine.game_object.components.texture_renderer;

namespace Worms.engine.core.renderer; 

public class TextureRendererHandler {
    private readonly IntPtr _renderer;
    private readonly GameSettings _settings;
    private readonly Dictionary<string, StoredTexture> _loadedTextures = new();

    public TextureRendererHandler(IntPtr renderer, GameSettings settings) {
        _renderer = renderer;
        _settings = settings;
    }

    public void RenderTextures(List<TextureRenderer> allActiveTextureRenderers) {
        allActiveTextureRenderers.ForEach(RenderTexture);
    }

    private unsafe void RenderTexture(TextureRenderer tr) {
        StoredTexture texture = GetTexture(tr);

        SDL.SDL_FRect destRect = WorldToScreenVectorCalculator.CalculateTextureDrawPosition(tr.Transform, texture.surface, _settings);
        float worldRotation = tr.Transform.WorldRotation.Value - _settings.camera.Rotation.Value;
        SDL.SDL_SetTextureColorMod(texture.texture, tr.color.Rbyte, tr.color.Gbyte, tr.color.Bbyte);
        SDL.SDL_RenderCopyExF(_renderer, texture.texture, IntPtr.Zero, ref destRect, worldRotation, IntPtr.Zero, GetTextureFlipSettings(tr));
    }
    
    private unsafe StoredTexture GetTexture(TextureRenderer tr) {
        if (!_loadedTextures.TryGetValue(tr.textureSrc, out StoredTexture? texture)) {
            texture = new StoredTexture((SDL.SDL_Surface *)SDL_image.IMG_Load(tr.textureSrc), SDL_image.IMG_LoadTexture(_renderer, tr.textureSrc));
            _loadedTextures.Add(tr.textureSrc, texture);
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