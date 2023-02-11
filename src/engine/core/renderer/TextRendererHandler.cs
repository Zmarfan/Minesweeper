using SDL2;
using Worms.engine.camera;
using Worms.engine.core.renderer.font;
using Worms.engine.data;
using Worms.engine.game_object.components.rendering;
using Worms.engine.game_object.components.rendering.text_renderer;
using Worms.engine.game_object.components.rendering.texture_renderer;

namespace Worms.engine.core.renderer; 

public static class TextRendererHandler {
    private static readonly int[] CHAR_INDICES = { 0, 1, 2, 2, 1, 3 };
    
    public static void RenderText(IntPtr renderer, Camera camera, Font font, TextRenderer tr, TransformationMatrix matrix) {
        Vector2 drawPosition = matrix.ConvertPoint(tr.Transform.Position);
        Vector2 pivot = drawPosition;
        float cameraModifier = 1 / camera.Size;
        SDL.SDL_Color color = new() { r = tr.color.Rbyte, g = tr.color.Gbyte, b = tr.color.Bbyte, a = tr.color.Abyte };

        foreach (char next in tr.text) {
            char c = font.characters.ContainsKey(next) ? next : '.';
            CharacterInfo info = font.characters[c];

            SDL.SDL_Vertex[] vertices = CreateCharacterVertices(drawPosition, color, info, font, cameraModifier, pivot, tr.Transform.Rotation);
            if (SDL.SDL_RenderGeometry(
                renderer,
                font.textureAtlas,
                vertices,
                vertices.Length,
                CHAR_INDICES,
                CHAR_INDICES.Length
            ) != 0) {
                throw new Exception($"Unable to render character due to: {SDL.SDL_GetError()}");
            }

            drawPosition.x += info.dimension.x * cameraModifier;
        }
    }

    private static SDL.SDL_Vertex[] CreateCharacterVertices(
        Vector2 topLeftPosition,
        SDL.SDL_Color color,
        CharacterInfo info,
        Font font,
        float cameraModifier,
        Vector2 pivot,
        Rotation rotation
    ) {
        return CalculateVertexPositions(topLeftPosition, info, font, cameraModifier)
            .Select(pos => RotateVertexPoint(pos, pivot, rotation))
            .Select((pos, i) => new SDL.SDL_Vertex { position = pos, color = color, tex_coord = info.textureCoords[i] })
            .ToArray();
    }

    private static IEnumerable<Vector2> CalculateVertexPositions(
        Vector2 topLeftPosition,
        CharacterInfo info,
        Font font,
        float cameraModifier
    ) {
        float charMaxHeightDiff = (font.maxCharHeight - info.dimension.y) * cameraModifier;
        return new List<Vector2> {
            new(topLeftPosition.x, topLeftPosition.y + charMaxHeightDiff),
            new(topLeftPosition.x + info.dimension.x * cameraModifier, topLeftPosition.y + charMaxHeightDiff),
            new(topLeftPosition.x, topLeftPosition.y + font.maxCharHeight * cameraModifier),
            new(topLeftPosition.x + info.dimension.x * cameraModifier,
                topLeftPosition.y + font.maxCharHeight * cameraModifier)
        };
    }
    
    private static SDL.SDL_FPoint RotateVertexPoint(Vector2 position, Vector2 pivot, Rotation rotation) {
        if (rotation == Rotation.Identity()) {
            return new SDL.SDL_FPoint { x = position.x, y = position.y };
        }

        Vector2 rotated = Vector2.RotatePointAroundPoint(position, pivot, rotation.Degree);
        return new SDL.SDL_FPoint { x = rotated.x, y = rotated.y };
    }
}