using SDL2;
using Worms.engine.core.input;
using Worms.engine.core.renderer;
using Worms.engine.core.update;
using EventHandler = Worms.engine.core.event_handler.EventHandler;

namespace Worms.engine.core; 

public class Game {
    private bool _isRunning;
    private readonly EventHandler _eventHandler;
    private readonly UpdateHandler _updateHandler;
    private readonly Renderer _renderer;

    public Game(GameSettings settings) {
        if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) != 0) {
            throw new Exception();
        }

        GameObjectHandler gameObjectHandler = new(settings.root);
        _renderer = new Renderer(settings, gameObjectHandler);
        _eventHandler = new EventHandler(settings);
        _eventHandler.QuitEvent += () => _isRunning = false;
        _updateHandler = new UpdateHandler(gameObjectHandler, settings.camera);
        Input.Init(_eventHandler, settings.inputListeners);
        
        _isRunning = true;
    }

    public void Run() {
        while (_isRunning) {
            _eventHandler.HandleEvents();
            _updateHandler.Awake();
            _updateHandler.Start();
            _updateHandler.Update();
            _renderer.Render();
        }
        Clean();
    }

    private void Clean() {
        _renderer.Clean();
        SDL.SDL_Quit();
    }
}