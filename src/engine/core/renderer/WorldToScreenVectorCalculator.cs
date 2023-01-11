using SDL2;
using Worms.engine.camera;
using Worms.engine.data;
using Worms.engine.game_object;

namespace Worms.engine.core.renderer; 

public static class WorldToScreenVectorCalculator {
    public static unsafe SDL.SDL_FRect CalculateTextureDrawPosition(Transform transform, SDL.SDL_Surface* surface, GameSettings settings) {
        float cameraSizeMod = 1 / settings.camera.Size;
        Vector2 screenPosition = CalculateScreenPosition(transform, surface, settings, cameraSizeMod);
        SDL.SDL_FRect rect = new() {
            x = screenPosition.x,
            y = screenPosition.y,
            w = surface->w * transform.WorldScale.x * (1 / settings.camera.Size),
            h = surface->h * transform.WorldScale.y * (1 / settings.camera.Size)
        };
        return rect;
    }

    private static unsafe Vector2 CalculateScreenPosition(Transform transform, SDL.SDL_Surface* surface, GameSettings settings, float cameraSizeMod) {
        Vector2 cameraOffsetPosition = transform.WorldPosition - settings.camera.Position;
        return new Vector2(
            settings.width / 2f + cameraOffsetPosition.x * cameraSizeMod - surface->w * transform.WorldScale.x / 2f * cameraSizeMod,
            settings.height / 2f - cameraOffsetPosition.y * cameraSizeMod - surface->h * transform.WorldScale.y / 2f * cameraSizeMod
        );
    }
}