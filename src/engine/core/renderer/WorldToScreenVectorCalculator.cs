﻿using SDL2;
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
            w = surface->w * transform.WorldScale * (1 / settings.camera.Size),
            h = surface->h * transform.WorldScale * (1 / settings.camera.Size)
        };
        return rect;
    }

    private static unsafe Vector2 CalculateScreenPosition(Transform transform, SDL.SDL_Surface* surface, GameSettings settings, float cameraSizeMod) {
        Vector2 cameraOffsetPosition = ConvertToCameraPosition(transform.WorldPosition, settings.camera);
        return new Vector2(
            settings.width / 2f + cameraOffsetPosition.x * cameraSizeMod - surface->w * transform.WorldScale / 2f * cameraSizeMod,
            settings.height / 2f - cameraOffsetPosition.y * cameraSizeMod - surface->h * transform.WorldScale / 2f * cameraSizeMod
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