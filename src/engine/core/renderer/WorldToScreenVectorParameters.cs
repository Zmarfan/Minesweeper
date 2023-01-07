using SDL2;
using Worms.engine.camera;
using Worms.engine.data;
using Worms.engine.game_object;

namespace Worms.engine.core.renderer; 

public class WorldToScreenVectorParameters {
    public Vector2 position;
    public float scale;
    public unsafe SDL.SDL_Surface* surface;
    public GameSettings settings;

    public unsafe WorldToScreenVectorParameters(Transform transform, SDL.SDL_Surface* surface, GameSettings settings) {
        position = transform.WorldPosition;
        scale = transform.WorldScale;
        this.surface = surface;
        this.settings = settings;
    }
}