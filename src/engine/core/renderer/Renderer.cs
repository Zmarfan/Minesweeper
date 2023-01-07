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

                SDL.SDL_Rect destRect = CalculateTextureDrawPosition(textureRenderer.Transform, texture.surface);
                SDL.SDL_RenderCopy(_renderer, texture.texture, IntPtr.Zero, ref destRect);
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
    
    private unsafe SDL.SDL_Rect CalculateTextureDrawPosition(Transform transform, SDL.SDL_Surface* sdlSurface) {
        Vector2 screenPosition = CalculateScreenPosition(transform.WorldPosition, sdlSurface);
        SDL.SDL_Rect rect = new();
        rect.x = (int)screenPosition.x;
        rect.y = (int)screenPosition.y;
        rect.w = (int)(sdlSurface->w * (1 /_settings.camera.Size));
        rect.h = (int)(sdlSurface->h * (1 /_settings.camera.Size));
        return rect;
    }

    private unsafe Vector2 CalculateScreenPosition(Vector2 position, SDL.SDL_Surface* sdlSurface) {
        return new Vector2(
            _settings.width / 2f + position.x * (1 /_settings.camera.Size) - sdlSurface->w / 2f * (1 /_settings.camera.Size),
            _settings.height / 2f - position.y * (1 /_settings.camera.Size) - sdlSurface->h / 2f * (1 /_settings.camera.Size)
        );
    }
}