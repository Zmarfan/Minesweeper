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
        tr.RefreshDataIfNeeded(font);
        
        float sizeModifier = 1 / camera.Size * tr.Size / Font.FONT_SIZE;
        UpdateVertexPositions(tr, sizeModifier, font, matrix);
        int[] indices = CreateIndicesFromVertices(tr.Vertices.Length);

        if (SDL.SDL_RenderGeometry(
            renderer,
            font.textureAtlas,
            tr.Vertices,
            tr.Vertices.Length,
            indices,
            indices.Length
        ) != 0) {
            throw new Exception($"Unable to render character due to: {SDL.SDL_GetError()}");
        }
    }

    private static void UpdateVertexPositions(
        TextRenderer tr,
        float sizeModifier,
        Font font,
        TransformationMatrix matrix
    ) {
        Vector2 drawPosition = matrix.ConvertPoint(tr.Transform.Position);
        Vector2 origin = drawPosition;

        int vertexIndex = 0;
        foreach (string line in tr.Lines.Where(line => line != string.Empty)) {
            foreach (CharacterInfo info in line.Select(c => font.characters[c])) {
                foreach (SDL.SDL_FPoint point in CalculateVertexPositions(drawPosition, info, font, sizeModifier, tr, origin)) {
                    tr.Vertices[vertexIndex++].position = point;
                }
                drawPosition.x += info.dimension.x * sizeModifier;
            }

            drawPosition.x = origin.x;
            drawPosition.y += font.maxCharHeight * sizeModifier;
        }
    }

    private static IEnumerable<SDL.SDL_FPoint> CalculateVertexPositions(
        Vector2 topLeftPosition,
        CharacterInfo info,
        Font font,
        float sizeModifier,
        TextRenderer tr,
        Vector2 origin
    ) {
        float charMaxHeightDiff = (font.maxCharHeight - info.dimension.y) * sizeModifier;
        float italicsOffset = tr.italics ? ITALICS_OFFSET * sizeModifier : 0;
        return new List<Vector2> {
            new(topLeftPosition.x + italicsOffset, topLeftPosition.y + charMaxHeightDiff),
            new(topLeftPosition.x + info.dimension.x * sizeModifier + italicsOffset, topLeftPosition.y + charMaxHeightDiff),
            new(topLeftPosition.x, topLeftPosition.y + font.maxCharHeight * sizeModifier),
            new(topLeftPosition.x + info.dimension.x * sizeModifier, topLeftPosition.y + font.maxCharHeight * sizeModifier)
        }.Select(pos => RotateVertexPoint(pos, origin, tr.Transform.Rotation));
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