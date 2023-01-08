using SDL2;
using Worms.engine.camera;
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
        Vector2 cameraOffsetPosition = ConvertToCameraPosition(p.position, p.settings.camera);
        return new Vector2(
            p.settings.width / 2f + cameraOffsetPosition.x * cameraSizeMod - p.surface->w * p.scale / 2f * cameraSizeMod,
            p.settings.height / 2f - cameraOffsetPosition.y * cameraSizeMod - p.surface->h * p.scale / 2f * cameraSizeMod
        );
    }

    private static Vector2 ConvertToCameraPosition(Vector2 position, Camera camera) {
        return RotatePointAroundPoint(position, camera.Position, camera.Rotation.Value) - camera.Position;
    }
    
    private static Vector2 RotatePointAroundPoint(Vector2 point, Vector2 pivot, float angle) {
        double radians = Math.PI * angle / 180;
        float s = (float)Math.Sin(radians);
        float c = (float)Math.Cos(radians);

        point -= pivot;
        point = new Vector2(point.x * c - point.y * s, point.x * s + point.y * c) + pivot;

        return point;
    }
}