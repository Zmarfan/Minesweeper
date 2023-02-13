using System.Text;
using SDL2;
using Worms.engine.camera;
using Worms.engine.core.renderer.font;
using Worms.engine.data;
using Worms.engine.game_object.components.rendering;
using Worms.engine.game_object.components.rendering.text_renderer;
using Worms.engine.game_object.components.rendering.texture_renderer;

namespace Worms.engine.core.renderer; 

public static class TextRendererHandler {
    private const int ITALICS_OFFSET = 20;
    
    public static void RenderText(IntPtr renderer, Camera camera, Font font, TextRenderer tr, TransformationMatrix matrix) {
        SDL.SDL_Vertex[] vertices = CreateTextVertices(camera, font, tr, matrix);
        int[] indices = CreateIndicesFromVertices(vertices.Length);

        if (SDL.SDL_RenderGeometry(
            renderer,
            font.textureAtlas,
            vertices.ToArray(),
            vertices.Length,
            indices,
            indices.Length
        ) != 0) {
            throw new Exception($"Unable to render character due to: {SDL.SDL_GetError()}");
        }
    }

    private static SDL.SDL_Vertex[] CreateTextVertices(
        Camera camera,
        Font font,
        TextRenderer tr,
        TransformationMatrix matrix
    ) {
        List<string> lines = tr.GetLines(font);
        Vector2 drawPosition = matrix.ConvertPoint(tr.Transform.Position);
        Vector2 origin = drawPosition;
        float sizeModifier = 1 / camera.Size * tr.size / Font.FONT_SIZE;
        SDL.SDL_Color color = new() { r = tr.color.Rbyte, g = tr.color.Gbyte, b = tr.color.Bbyte, a = tr.color.Abyte };
        Vector2 boldPosition = new(drawPosition.x + 2 * sizeModifier, drawPosition.y);

        List<SDL.SDL_Vertex> vertices = new();
        foreach (string line in lines) {
            if (line != string.Empty) {
                vertices.AddRange(CreateVerticesForLine(
                    line,
                    drawPosition,
                    origin,
                    tr,
                    sizeModifier,
                    color,
                    font
                ));
                if (tr.bold) {
                    vertices.AddRange(CreateVerticesForLine(
                        line,
                        boldPosition,
                        origin,
                        tr,
                        sizeModifier,
                        color,
                        font
                    ));
                }
            }
            drawPosition.y += font.maxCharHeight * sizeModifier;
            boldPosition.y += font.maxCharHeight * sizeModifier;
        }
        
        return vertices.ToArray();
    }

    private static IEnumerable<SDL.SDL_Vertex> CreateVerticesForLine(
        string line,
        Vector2 drawPosition,
        Vector2 origin,
        TextRenderer tr,
        float sizeModifier,
        SDL.SDL_Color color,
        Font font
    ) {
        List<SDL.SDL_Vertex> vertices = new();
        foreach (CharacterInfo info in line.Select(c => font.characters[c])) {
            vertices.AddRange(CreateCharacterVertices(
                CalculateVertexPositions(drawPosition, info, font, sizeModifier, tr.italics),
                color,
                info,
                origin,
                tr.Transform.Rotation
            ));
            drawPosition.x += info.dimension.x * sizeModifier;
        }

        return vertices;
    }

    private static IEnumerable<SDL.SDL_Vertex> CreateCharacterVertices(
        IEnumerable<Vector2> vertexPositions,
        SDL.SDL_Color color,
        CharacterInfo info,
        Vector2 origin,
        Rotation rotation
    ) {
        return vertexPositions
            .Select(pos => RotateVertexPoint(pos, origin, rotation))
            .Select((pos, i) => new SDL.SDL_Vertex { position = pos, color = color, tex_coord = info.textureCoords[i] })
            .ToList();
    }

    private static IEnumerable<Vector2> CalculateVertexPositions(
        Vector2 topLeftPosition,
        CharacterInfo info,
        Font font,
        float sizeModifier,
        bool italics
    ) {
        float charMaxHeightDiff = (font.maxCharHeight - info.dimension.y) * sizeModifier;
        float italicsOffset = italics ? ITALICS_OFFSET * sizeModifier : 0;
        return new List<Vector2> {
            new(topLeftPosition.x + italicsOffset, topLeftPosition.y + charMaxHeightDiff),
            new(topLeftPosition.x + info.dimension.x * sizeModifier + italicsOffset, topLeftPosition.y + charMaxHeightDiff),
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