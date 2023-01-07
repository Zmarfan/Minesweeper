using SDL2;

namespace Worms.engine.core; 

public class Renderer {
    private IntPtr _window;
    private IntPtr _renderer;

    public Renderer(string title, int width, int height) {
        _window = SDL.SDL_CreateWindow(
            title,
            SDL.SDL_WINDOWPOS_CENTERED, 
            SDL.SDL_WINDOWPOS_CENTERED,
            width, 
            height,
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN
        );
        if (_window == IntPtr.Zero) {
            throw new Exception();
        }

        _renderer = SDL.SDL_CreateRenderer(_window, -1, 0);
        if (_renderer == IntPtr.Zero) {
            throw new Exception();
        }
    }
    
    public void Render() {
        SDL.SDL_RenderClear(_renderer);
        SDL.SDL_RenderPresent(_renderer);
    }

    public void Clean() {
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_DestroyRenderer(_renderer);
    }
}