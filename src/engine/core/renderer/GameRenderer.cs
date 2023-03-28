using SDL2;
using GameEngine.engine.camera;
using GameEngine.engine.core.cursor;
using GameEngine.engine.core.renderer.font;
using GameEngine.engine.core.renderer.textures;
using GameEngine.engine.core.window;
using GameEngine.engine.data;
using GameEngine.engine.scene;
using GameEngine.engine.window;
using GameEngine.engine.window.menu;

namespace GameEngine.engine.core.renderer; 

internal class GameRenderer {
    private readonly nint _window;
    private readonly nint _renderer;
    private readonly TextureStorage _textureStorage;
    private readonly FontHandler _fontHandler;
    private readonly RendererHandler _rendererHandler;
    private readonly GizmosRendererHandler _gizmosRendererHandler;
    private readonly SceneData _sceneData;
    private readonly GameSettings _settings;
    private static Color DefaultDrawColor => Camera.Main.defaultDrawColor;
    private bool _isFullscreen;
    private nint? _iconSurface;
    
    public GameRenderer(GameSettings settings, SceneData sceneData) {
        _window = SDL.SDL_CreateWindow(
            settings.title,
            SDL.SDL_WINDOWPOS_CENTERED, 
            SDL.SDL_WINDOWPOS_CENTERED,
            settings.width, 
            settings.height,
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN
        );
        SDL.SDL_SetHint( SDL.SDL_HINT_RENDER_SCALE_QUALITY, "1");
        if (_window == nint.Zero) {
            throw new Exception();
        }

        _renderer = SDL.SDL_CreateRenderer(_window, -1, 0);
        if (_renderer == nint.Zero) {
            throw new Exception();
        }
        SDL.SDL_SetWindowGrab(_window, SDL.SDL_bool.SDL_TRUE);
        
        WindowManager.Init(_window, settings);

        if (settings.iconSrc != null) {
            InitWindowIcon(settings.iconSrc);
        }
        
        _settings = settings;
        _sceneData = sceneData;
        WindowHandler.Init(_window, WindowMenuBuilder
            .Builder("menu")
            .AddDropDown(WindowMenuBuilder
                .Builder("dropdown")
                .AddItem(MenuItemBuilder.Builder("1", "drop 1").IsChecked(true).Build())
                .AddItem(MenuItemBuilder.Builder("2", "drop 2").Build())
                .AddDropDown(WindowMenuBuilder
                    .Builder("dropdown 2")
                    .AddItem(MenuItemBuilder.Builder("3", "entry 1").IsChecked(true).Build())
                    .AddItem(MenuItemBuilder.Builder("4", "entry 2").IsDisabled(true).Build())
                    .AddBreak()
                    .AddItem(MenuItemBuilder.Builder("5", "entry 3").Build())
                    .Build()
                )
                .AddItem(MenuItemBuilder.Builder("6", "asdasd").Build())
                .Build()
            )
            .AddItem(MenuItemBuilder.Builder("7", "123 2").Build())
            .Build()
        );
        _textureStorage = TextureStorage.Init(_renderer, _settings.assets.textureDeclarations);
        _fontHandler = new FontHandler(_renderer, settings.assets.fontDeclarations);
        _rendererHandler = new RendererHandler(_renderer, _fontHandler, settings);
        _gizmosRendererHandler = new GizmosRendererHandler(_renderer);
        Cursor.Init(settings.cursorSettings, _window);
    }

    public void Render() {
        SetDrawColor(DefaultDrawColor);
        if (SDL.SDL_RenderClear(_renderer) != 0) {
            throw new Exception($"Unable to clear renderer due to: {SDL.SDL_GetError()}");
        }
        _rendererHandler.Render(_sceneData.gameObjectHandler.objects);
        if (_settings.debug) {
            _gizmosRendererHandler.RenderGizmos(_sceneData.gameObjectHandler.objects);
        }

        SDL.SDL_RenderPresent(_renderer);
    }

    public void ToggleFullScreen() {
        // Currently not supported, it's weird with resolution
        return;
        uint flag = _isFullscreen ? 0 : (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN;
        if (SDL.SDL_SetWindowFullscreen(_window, flag) != 0) {
            throw new Exception($"Unable to change window fullScreen mode due to: {SDL.SDL_GetError()}");
        }
        _isFullscreen = !_isFullscreen;
    }
    
    private void InitWindowIcon(string iconSrc) {
        _iconSurface = SDL_image.IMG_Load(iconSrc);
        SDL.SDL_SetWindowIcon(_window, _iconSurface.Value);
    }
    
    public void Clean() {
        Cursor.Clean();
        _fontHandler.Clean();
        _textureStorage.Clean();
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_DestroyRenderer(_renderer);
        if (_iconSurface.HasValue) {
            SDL.SDL_FreeSurface(_iconSurface.Value);
        } 
    }

    private void SetDrawColor(Color c) {
        if (SDL.SDL_SetRenderDrawColor(_renderer, c.Rbyte, c.Gbyte, c.Bbyte, c.Abyte) != 0) {
            throw new Exception($"Unable to set render draw color due to: {SDL.SDL_GetError()}");
        }
    }
}