using SDL2;
using Worms.engine.camera;
using Worms.engine.core.renderer.font;
using Worms.engine.data;
using Worms.engine.game_object.components.rendering;
using Worms.engine.game_object.components.rendering.text_renderer;
using Worms.engine.game_object.components.rendering.texture_renderer;

namespace Worms.engine.core.renderer; 

public static class TextRendererHandler {
    public static void RenderText(IntPtr renderer, Camera camera, Font font, TextRenderer tr, TransformationMatrix matrix) {
        Vector2 drawPosition = matrix.ConvertPoint(tr.Transform.Position);
        Vector2 pivot = drawPosition;
        float sizeModifier = 1 / camera.Size * tr.size / Font.FONT_SIZE;
        SDL.SDL_Color color = new() { r = tr.color.Rbyte, g = tr.color.Gbyte, b = tr.color.Bbyte, a = tr.color.Abyte };

        List<SDL.SDL_Vertex> vertices = new();
        foreach (char next in tr.text) {
            char c = font.characters.ContainsKey(next) ? next : '.';
            CharacterInfo info = font.characters[c];

            vertices.AddRange(CreateCharacterVertices(
                CalculateVertexPositions(drawPosition, info, font, sizeModifier),
                color,
                info,
                pivot,
                tr.Transform.Rotation
            ));
            drawPosition.x += info.dimension.x * sizeModifier;
        }

        int[] indices = CreateIndicesFromVertices(vertices.Count);
        if (SDL.SDL_RenderGeometry(
            renderer,
            font.textureAtlas,
            vertices.ToArray(),
            vertices.Count,
            indices,
            indices.Length
        ) != 0) {
            throw new Exception($"Unable to render character due to: {SDL.SDL_GetError()}");
        }
    }

    private static List<SDL.SDL_Vertex> CreateCharacterVertices(
        IEnumerable<Vector2> vertexPositions,
        SDL.SDL_Color color,
        CharacterInfo info,
        Vector2 pivot,
        Rotation rotation
    ) {
        return vertexPositions
            .Select(pos => RotateVertexPoint(pos, pivot, rotation))
            .Select((pos, i) => new SDL.SDL_Vertex { position = pos, color = color, tex_coord = info.textureCoords[i] })
            .ToList();
    }

    private static IEnumerable<Vector2> CalculateVertexPositions(
        Vector2 topLeftPosition,
        CharacterInfo info,
        Font font,
        float sizeModifier
    ) {
        float charMaxHeightDiff = (font.maxCharHeight - info.dimension.y) * sizeModifier;
        return new List<Vector2> {
            new(topLeftPosition.x, topLeftPosition.y + charMaxHeightDiff),
            new(topLeftPosition.x + info.dimension.x * sizeModifier, topLeftPosition.y + charMaxHeightDiff),
            new(topLeftPosition.x, topLeftPosition.y + font.maxCharHeight * sizeModifier),
            new(topLeftPosition.x + info.dimension.x * sizeModifier, topLeftPosition.y + font.maxCharHeight * sizeModifier)
        };
    }
    
    private static SDL.SDL_FPoint RotateVertexPoint(Vector2 position, Vector2 pivot, Rotation rotation) {
        if (rotation == Rotation.Identity()) {
            return new SDL.SDL_FPoint { x = position.x, y = position.y };
        }

        Vector2 rotated = Vector2.RotatePointAroundPoint(position, pivot, rotation.Degree);
        return new SDL.SDL_FPoint { x = rotated.x, y = rotated.y };
    }
    
    private static int[] CreateIndicesFromVertices(int totalVertices) {
        int totalCharacters = totalVertices / 4;
        int[] indices = new int[totalCharacters * 6];
        for (int i = 0; i < totalCharacters; i++) {
            int first = i * 4;
            indices[i * 6] = first;
            indices[i * 6 + 1] = first + 1;
            indices[i * 6 + 2] = first + 2;
            indices[i * 6 + 3] = first + 2;
            indices[i * 6 + 4] = first + 1;
            indices[i * 6 + 5] = first + 3;
        }

        return indices;
    }
}