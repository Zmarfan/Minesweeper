using SDL2;
using Worms.engine.core.audio;
using Worms.engine.core.input;
using Worms.engine.core.renderer;
using Worms.engine.core.update;
using EventHandler = Worms.engine.core.event_handler.EventHandler;

namespace Worms.engine.core; 

public class Game {
    private const float MAX_FPS = 120;

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
        _eventHandler.ToggleFullscreenEvent += _renderer.ToggleFullScreen;
        _updateHandler = new UpdateHandler(gameObjectHandler, settings.camera);
        AudioHandler.Init(settings.audioSettings);
        Input.Init(_eventHandler, settings.inputListeners);
        
        _isRunning = true;
    }

    public void Run() {
        while (_isRunning) {
            ulong start = SDL.SDL_GetPerformanceCounter();
            
            RunFrame();
    
            double elapsedMs = (SDL.SDL_GetPerformanceCounter() - start) / (double)SDL.SDL_GetPerformanceFrequency() * 1000.0f;
            SDL.SDL_Delay((uint)Math.Max(1000 / MAX_FPS - elapsedMs, 0));
        }
        Clean();
    }

    private void RunFrame() {
        try {
            _eventHandler.HandleEvents();
            _updateHandler.Awake();
            _updateHandler.Start();
            _updateHandler.UpdateLoops();
            _updateHandler.EndOfFrameCleanUp();
            _renderer.Render();
        }
        catch (Exception e) {
            Console.WriteLine($"An issue occured during this frame the exception was: {e}");
        }
    }
    
    private void Clean() {
        AudioHandler.Clean();
        _renderer.Clean();
        SDL.SDL_Quit();
    }
}