using SDL2;
using Worms.engine.camera;
using Worms.engine.game_object;
using Worms.engine.game_object.components.texture_renderer;

namespace Worms.engine.core.renderer; 

public class Renderer {
    private IntPtr _window;
    private IntPtr _renderer;
    private readonly GameObject _root;
    private readonly Camera _camera;

    private readonly Dictionary<string, StoredTexture> _loadedTextures = new();

    public Renderer(GameSettings settings) {
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

        _root = settings.root;
        _camera = settings.camera;
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
        GameObjectHelper.GetAllComponentsOfTypeFromGameObject<TextureRenderer>(_root, true)
            .ToList()
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
        SDL.SDL_Rect rect = new();
        rect.x = (int)transform.WorldPosition.x;
        rect.y = (int)-transform.WorldPosition.y;
        rect.w = sdlSurface->w;
        rect.h = sdlSurface->h;
        return rect;
    }
}