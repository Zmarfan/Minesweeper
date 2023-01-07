using SDL2;
using Worms.engine.core.renderer;

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

        _renderer = new Renderer(settings);
        _eventHandler = new EventHandler(() => _isRunning = false);
        _updateHandler = new UpdateHandler(settings.root);

        _isRunning = true;
    }

    public void Run() {
        while (_isRunning) {
            _eventHandler.HandleEvents();
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