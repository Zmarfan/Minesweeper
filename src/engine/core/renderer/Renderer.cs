using SDL2;
using Worms.engine.camera;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.texture_renderer;

namespace Worms.engine.core.renderer; 

public class Renderer {
    private IntPtr _window;
    private IntPtr _renderer;
    private readonly GameSettings _settings;

    private readonly Dictionary<string, StoredTexture> _loadedTextures = new();

    private readonly GameObjectHandler _gameObjectHandler;
    
    public Renderer(GameSettings settings, GameObjectHandler gameObjectHandler) {
        _window = SDL.SDL_CreateWindow(
            settings.title,
            SDL.SDL_WINDOWPOS_CENTERED, 
            SDL.SDL_WINDOWPOS_CENTERED,
            settings.width, 
            settings.height,
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN
        );
        if (_window == IntPtr.Zero) {
            throw new Exception();
        }

        _renderer = SDL.SDL_CreateRenderer(_window, -1, 0);
        if (_renderer == IntPtr.Zero) {
            throw new Exception();
        }
        SDL.SDL_SetHint(  SDL.SDL_HINT_RENDER_SCALE_QUALITY, "1" );
        
        _settings = settings;
        _gameObjectHandler = gameObjectHandler;
    }

    public void Render() {
        SDL.SDL_RenderClear(_renderer);
        RenderTextures();
        SDL.SDL_RenderPresent(_renderer);
    }

    public unsafe void Clean() {
        _loadedTextures.Values.ToList().ForEach(texture => {
            SDL.SDL_FreeSurface((nint)texture.surface);
            SDL.SDL_DestroyTexture(texture.texture);
        });
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_DestroyRenderer(_renderer);
    }

    private unsafe void RenderTextures() {
        _gameObjectHandler.AllActiveTextureRenderers
            .ForEach(textureRenderer => {
                StoredTexture texture = GetTexture(textureRenderer);

                SDL.SDL_Rect destRect = WorldToScreenVectorCalculator.CalculateTextureDrawPosition(new WorldToScreenVectorParameters(textureRenderer.Transform, texture.surface, _settings));
                float worldRotation = textureRenderer.Transform.WorldRotation.Value;
                SDL.SDL_RenderCopyEx(_renderer, texture.texture, IntPtr.Zero, ref destRect, worldRotation, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            });
    }

    private unsafe StoredTexture GetTexture(TextureRenderer tr) {
        if (!_loadedTextures.TryGetValue(tr.textureSrc, out StoredTexture? texture)) {
            texture = new StoredTexture((SDL.SDL_Surface *)SDL_image.IMG_Load(tr.textureSrc), SDL_image.IMG_LoadTexture(_renderer, tr.textureSrc));
            _loadedTextures.Add(tr.textureSrc, texture);
            Console.WriteLine($"Loaded: {tr.textureSrc}");
        }

        return texture;
    }
}