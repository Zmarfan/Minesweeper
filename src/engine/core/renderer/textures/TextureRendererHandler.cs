using SDL2;
using Worms.engine.data;
using Worms.engine.game_object.components.rendering.texture_renderer;

namespace Worms.engine.core.renderer.textures; 

public static class TextureRendererHandler {
    public static unsafe void RenderTexture(IntPtr renderer, TextureRenderer tr, TransformationMatrix matrix) {
        StoredTexture texture = TextureStorage.GetAndCacheTexture(renderer, tr.texture);

        SDL.SDL_Rect srcRect = tr.texture.GetSrcRect(texture);
        SDL.SDL_FRect destRect = CalculateTextureDrawPosition(tr, texture.surface, matrix);
        if (SDL.SDL_SetTextureColorMod(texture.texture, tr.Color.Rbyte, tr.Color.Gbyte, tr.Color.Bbyte) != 0) {
            throw new Exception($"Unable to set texture color mod due to: {SDL.SDL_GetError()}");
        }

        if (SDL.SDL_SetTextureAlphaMod(texture.texture, tr.Color.Abyte) != 0) {
            throw new Exception($"Unable to set texture alpha mod due to: {SDL.SDL_GetError()}");
        }

        if (SDL.SDL_RenderCopyExF(
            renderer,
            texture.texture,
            ref srcRect,
            ref destRect,
            tr.Transform.Rotation.Degree, 
            IntPtr.Zero,
            GetTextureFlipSettings(tr)
        ) != 0) {
            throw new Exception($"Unable to render texture to screen due to: {SDL.SDL_GetError()}");
        }
    }

    private static unsafe SDL.SDL_FRect CalculateTextureDrawPosition(TextureRenderer tr, SDL.SDL_Surface* surface, TransformationMatrix matrix) {
        Vector2 screenPosition = CalculateTextureScreenPosition(tr, surface, matrix);
        Vector2 textureDimensions = CalculateTextureDimensions(tr, surface, matrix);
        SDL.SDL_FRect rect = new() {
            x = screenPosition.x,
            y = screenPosition.y,
            w = textureDimensions.x,
            h = textureDimensions.y
        };
        return rect;
    }

    private static unsafe Vector2 CalculateTextureScreenPosition(TextureRenderer tr, SDL.SDL_Surface* surface, TransformationMatrix matrix) {
        return matrix.ConvertPoint(tr.Transform.Position) - CalculateTextureDimensions(tr, surface, matrix) / 2f + CalculatePixelPerfectOffset(tr, surface, matrix) / 2f;
    }

    private static unsafe Vector2 CalculateTextureDimensions(TextureRenderer tr, SDL.SDL_Surface* surface, TransformationMatrix matrix) {
        return matrix.ConvertVector(
            new Vector2(surface->w * tr.Transform.Scale.x, surface->h * tr.Transform.Scale.y * -1) * tr.texture.textureScale
        );
    }
    
    private static unsafe Vector2 CalculatePixelPerfectOffset(TextureRenderer tr, SDL.SDL_Surface* surface, TransformationMatrix matrix) {
        return matrix.ConvertVector(new Vector2(
            (surface->w * tr.texture.textureScale.x + 1) % 2, 
            (surface->h * tr.texture.textureScale.y + 1) % 2 * -1
        ));
    }
    
    private static SDL.SDL_RendererFlip GetTextureFlipSettings(TextureRenderer tr) {
        SDL.SDL_RendererFlip flip = SDL.SDL_RendererFlip.SDL_FLIP_NONE;
        if (tr.flipX) {
            flip |= SDL.SDL_RendererFlip.SDL_FLIP_HORIZONTAL;
        }
        if (tr.flipY) {
            flip |= SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL;
        }
        return flip;
    }
}