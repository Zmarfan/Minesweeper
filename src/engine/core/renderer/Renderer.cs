using SDL2;
using Worms.engine.camera;
using Worms.engine.data;
using Worms.engine.scene;

namespace Worms.engine.core.renderer; 

public class Renderer {
    private readonly IntPtr _window;
    private readonly IntPtr _renderer;
    private readonly TextureRendererHandler _textureRendererHandler;
    private readonly GizmosRendererHandler _gizmosRendererHandler;
    private readonly SceneData _sceneData;
    private readonly GameSettings _settings;
    private Color DefaultDrawColor => _sceneData.camera.defaultDrawColor;
    private bool _isFullscreen = false;
    
    public Renderer(GameSettings settings, SceneData sceneData) {
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
        SDL.SDL_SetWindowGrab(_window, SDL.SDL_bool.SDL_TRUE);
        
        _settings = settings;
        _sceneData = sceneData;
        _textureRendererHandler = new TextureRendererHandler(_renderer, settings, _sceneData);
        _gizmosRendererHandler = new GizmosRendererHandler(_renderer, _sceneData);
    }

    public void Render() {
         SetDrawColor(DefaultDrawColor);
        SDL.SDL_RenderClear(_renderer);
        _textureRendererHandler.RenderTextures(_sceneData.gameObjectHandler.AllActiveGameObjectTextureRenderers);
        if (_settings.debug) {
            _gizmosRendererHandler.RenderGizmos(_sceneData.gameObjectHandler.AllActiveGameObjectScripts);
        }
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

    private void SetDrawColor(Color c) {
        SDL.SDL_SetRenderDrawColor(_renderer, c.Rbyte, c.Gbyte, c.Bbyte, c.Abyte);
    }
}