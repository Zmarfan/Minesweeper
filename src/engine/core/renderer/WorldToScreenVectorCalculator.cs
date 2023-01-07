using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.renderer; 

public static class WorldToScreenVectorCalculator {
    public static unsafe SDL.SDL_FRect CalculateTextureDrawPosition(WorldToScreenVectorParameters p) {
        float cameraSizeMod = 1 / p.settings.camera.Size;
        Vector2 screenPosition = CalculateScreenPosition(p, cameraSizeMod);
        SDL.SDL_FRect rect = new() {
            x = screenPosition.x,
            y = screenPosition.y,
            w = p.surface->w * p.scale * (1 /p.settings.camera.Size),
            h = p.surface->h * p.scale * (1 /p.settings.camera.Size)
        };
        return rect;
    }

    private static unsafe Vector2 CalculateScreenPosition(WorldToScreenVectorParameters p, float cameraSizeMod) {
        Vector2 cameraOffsetPosition = p.position - p.settings.camera.Position;
        return new Vector2(
            p.settings.width / 2f + cameraOffsetPosition.x * cameraSizeMod - p.surface->w * p.scale / 2f * cameraSizeMod,
            p.settings.height / 2f - cameraOffsetPosition.y * cameraSizeMod - p.surface->h * p.scale / 2f * cameraSizeMod
        );
    }
}