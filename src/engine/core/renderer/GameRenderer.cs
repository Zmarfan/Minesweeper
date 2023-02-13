﻿using SDL2;
using Worms.engine.camera;
using Worms.engine.core.renderer.font;
using Worms.engine.core.window;
using Worms.engine.data;
using Worms.engine.scene;

namespace Worms.engine.core.renderer; 

public class GameRenderer {
    private readonly IntPtr _window;
    private readonly IntPtr _renderer;
    private readonly FontHandler _fontHandler;
    private readonly RendererHandler _rendererHandler;
    private readonly GizmosRendererHandler _gizmosRendererHandler;
    private readonly SceneData _sceneData;
    private readonly GameSettings _settings;
    private Color DefaultDrawColor => _sceneData.camera.defaultDrawColor;
    private bool _isFullscreen = false;
    
    public GameRenderer(GameSettings settings, SceneData sceneData) {
        _window = SDL.SDL_CreateWindow(
            settings.title,
            SDL.SDL_WINDOWPOS_CENTERED, 
            SDL.SDL_WINDOWPOS_CENTERED,
            settings.width, 
            settings.height,
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN
        );
        // SDL.SDL_SetHint( SDL.SDL_HINT_RENDER_SCALE_QUALITY, "1" );
        if (_window == IntPtr.Zero) {
            throw new Exception();
        }

        _renderer = SDL.SDL_CreateRenderer(_window, -1, 0);
        if (_renderer == IntPtr.Zero) {
            throw new Exception();
        }
        SDL.SDL_SetWindowGrab(_window, SDL.SDL_bool.SDL_TRUE);
        
        WindowManager.Init(_window, settings);
        
        _settings = settings;
        _sceneData = sceneData;
        _fontHandler = new FontHandler(_renderer, settings.fontDefinitions);
        _rendererHandler = new RendererHandler(_renderer, _fontHandler, settings, _sceneData);
        _gizmosRendererHandler = new GizmosRendererHandler(_renderer, _sceneData);
    }

    public void Render() {
         SetDrawColor(DefaultDrawColor);
        SDL.SDL_RenderClear(_renderer);
        _rendererHandler.Render(_sceneData.gameObjectHandler.objects);
        if (_settings.debug) {
            _gizmosRendererHandler.RenderGizmos(_sceneData.gameObjectHandler.objects);
        }

        SDL.SDL_RenderPresent(_renderer);
    }

    public void ToggleFullScreen() {
        uint flag = _isFullscreen ? 0 : (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN;
        SDL.SDL_SetWindowFullscreen(_window, flag);
        _isFullscreen = !_isFullscreen;
    }
    
    public void Clean() {
        _fontHandler.Clean();
        TextureStorage.Clean();
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_DestroyRenderer(_renderer);
    }

    private void SetDrawColor(Color c) {
        SDL.SDL_SetRenderDrawColor(_renderer, c.Rbyte, c.Gbyte, c.Bbyte, c.Abyte);
    }
}