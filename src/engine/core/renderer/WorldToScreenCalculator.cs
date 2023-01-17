using SDL2;
using Worms.engine.camera;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.texture_renderer;

namespace Worms.engine.core.renderer; 

public static class WorldToScreenCalculator {
    public static Vector2 WorldToScreenPosition(Vector2 worldPosition, GameSettings settings) {
        Vector2 cameraOffsetPosition = (worldPosition - settings.camera.Position) * CameraSizeMod(settings);
        return new Vector2(settings.width / 2f + cameraOffsetPosition.x, settings.height / 2f - cameraOffsetPosition.y);
    }
    
    public static Vector2 WorldToScreenVector(Vector2 worldVector, GameSettings settings) {
        return worldVector * CameraSizeMod(settings);
    }
    
    public static unsafe SDL.SDL_FRect CalculateTextureDrawPosition(TextureRenderer tr, SDL.SDL_Surface* surface, GameSettings settings) {
        Vector2 screenPosition = CalculateTextureScreenPosition(tr, surface, settings);
        Vector2 textureDimensions = CalculateTextureDimensions(tr, surface, settings);
        SDL.SDL_FRect rect = new() {
            x = screenPosition.x,
            y = screenPosition.y,
            w = textureDimensions.x,
            h = textureDimensions.y
        };
        return rect;
    }

    private static unsafe Vector2 CalculateTextureScreenPosition(TextureRenderer tr, SDL.SDL_Surface* surface, GameSettings settings) {
        return WorldToScreenPosition(tr.Transform.Position, settings) - CalculateTextureDimensions(tr, surface, settings) / 2f;
    }

    private static unsafe Vector2 CalculateTextureDimensions(TextureRenderer tr, SDL.SDL_Surface* surface, GameSettings settings) {
        return new Vector2(surface->w * tr.Transform.Scale.x, surface->h * tr.Transform.Scale.y) * CameraSizeMod(settings) * tr.texture.textureScale;
    }
    
    private static float CameraSizeMod(GameSettings settings) {
        return 1 / settings.camera.Size;
    }
}