using SDL2;

namespace Worms.engine.core; 

public class Game {
    private bool _isRunning;
    private readonly EventHandler _eventHandler;
    private readonly UpdateHandler _updateHandler = new();
    private readonly Renderer _renderer;

    public Game(string title, int width, int height) {
        if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) != 0) {
            throw new Exception();
        }

        _renderer = new Renderer(title, width, height);
        _eventHandler = new EventHandler(() => _isRunning = false);

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