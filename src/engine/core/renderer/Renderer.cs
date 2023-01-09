using SDL2;
using Worms.engine.data;
using Worms.engine.game_object.components.texture_renderer;

namespace Worms.engine.core.renderer; 

public class Renderer {
    private readonly IntPtr _window;
    private readonly IntPtr _renderer;
    private readonly TextureRendererHandler _textureRendererHandler;
    private readonly GameSettings _settings;
    private Color DefaultDrawColor => _settings.camera.defaultDrawColor;
    private bool _isFullscreen = false;

    private readonly GameObjectHandler _gameObjectHandler;
    
    public Renderer(GameSettings settings, GameObjectHandler gameObjectHandler) {
        _window = SDL.SDL_CreateWindow(
            settings.title,
            SDL.SDL_WINDOWPOS_CENTERED, 
            SDL.SDL_WINDOWPOS_CENTERED,
            settings.width, 
            settings.height,
            SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE
        );
        if (_window == IntPtr.Zero) {
            throw new Exception();
        }

        _renderer = SDL.SDL_CreateRenderer(_window, -1, 0);
        if (_renderer == IntPtr.Zero) {
            throw new Exception();
        }
        SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "1" );
        SDL.SDL_SetRelativeMouseMode(SDL.SDL_bool.SDL_TRUE);
        
        _settings = settings;
        _gameObjectHandler = gameObjectHandler;
        _textureRendererHandler = new TextureRendererHandler(_renderer, settings);
    }

    public void Render() {
        SDL.SDL_RenderClear(_renderer);
        DrawBackground(DefaultDrawColor);
        _textureRendererHandler.RenderTextures(_gameObjectHandler.AllActiveGameObjectTextureRenderers);
        SDL.SDL_RenderPresent(_renderer);
    }

    public void ToggleFullScreen() {
        uint flag = _isFullscreen ? 0 : (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN;
        SDL.SDL_SetWindowFullscreen(_window, flag);
        _isFullscreen = !_isFullscreen;
    }
    
    public void Clean() {
        _textureRendererHandler.Clean();
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_DestroyRenderer(_renderer);
    }

    private void DrawBackground(Color c) {
        SDL.SDL_SetRenderDrawColor(_renderer, c.Rbyte, c.Gbyte, c.Bbyte, c.Abyte);
    }
}