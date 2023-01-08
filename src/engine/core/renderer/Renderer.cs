using System.Drawing;
using SDL2;

namespace Worms.engine.core.renderer; 

public class Renderer {
    private IntPtr _window;
    private IntPtr _renderer;
    private readonly TextureRendererHandler _textureRendererHandler;
    private readonly GameSettings _settings;
    private Color DefaultDrawColor => _settings.camera.defaultDrawColor;

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
        _textureRendererHandler = new TextureRendererHandler(_renderer, settings);
    }

    public void Render() {
        SDL.SDL_RenderClear(_renderer);
        SDL.SDL_SetRenderDrawColor(_renderer, DefaultDrawColor.R, DefaultDrawColor.G, DefaultDrawColor.B, DefaultDrawColor.A);
        _textureRendererHandler.RenderTextures(_gameObjectHandler.AllActiveTextureRenderers);
        SDL.SDL_RenderPresent(_renderer);
    }

    public void Clean() {
        _textureRendererHandler.Clean();
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_DestroyRenderer(_renderer);
    }
}