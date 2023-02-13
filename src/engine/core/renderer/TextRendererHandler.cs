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
    private const int BOLD_OFFSET = 3;
    
    public static void RenderText(IntPtr renderer, Camera camera, Font font, TextRenderer tr, TransformationMatrix matrix) {
        tr.RefreshDataIfNeeded(font);
        
        Vector2 sizeModifier = 1 / camera.Size * tr.Size / Font.FONT_SIZE * tr.Transform.Scale;
        UpdateVertexPositions(tr, sizeModifier, font, matrix.ConvertPoint(tr.Transform.Position));

        RenderTextGeometry(renderer, tr, font);
        if (tr.bold) {
            RenderBoldText(renderer, tr, font, sizeModifier);
        }
    }

    private static void UpdateVertexPositions(
        TextRenderer tr,
        Vector2 sizeModifier,
        Font font,
        Vector2 origin
    ) {
        Vector2 drawPosition = origin;

        int vertexIndex = 0;
        foreach (string line in tr.Lines) {
            if (line != string.Empty) {
                foreach (CharacterInfo info in line.Select(c => font.characters[c])) {
                    foreach (SDL.SDL_FPoint point in CalculateVertexPositions(drawPosition, info, font, sizeModifier, tr, origin)) {
                        tr.Vertices[vertexIndex++].position = point;
                    }
                    drawPosition.x += info.dimension.x * sizeModifier.x;
                }
            }

            drawPosition.x = origin.x;
            drawPosition.y += font.maxCharHeight * sizeModifier.y;
        }
    }

    private static IEnumerable<SDL.SDL_FPoint> CalculateVertexPositions(
        Vector2 topLeftPosition,
        CharacterInfo info,
        Font font,
        Vector2 sizeModifier,
        TextRenderer tr,
        Vector2 origin
    ) {
        float charMaxHeightDiff = (font.maxCharHeight - info.dimension.y) * sizeModifier.y;
        float italicsOffset = tr.italics ? ITALICS_OFFSET * sizeModifier.x : 0;
        return new List<Vector2> {
            new(topLeftPosition.x + italicsOffset, topLeftPosition.y + charMaxHeightDiff),
            new(topLeftPosition.x + info.dimension.x * sizeModifier.x + italicsOffset, topLeftPosition.y + charMaxHeightDiff),
            new(topLeftPosition.x, topLeftPosition.y + font.maxCharHeight * sizeModifier.y),
            new(topLeftPosition.x + info.dimension.x * sizeModifier.x, topLeftPosition.y + font.maxCharHeight * sizeModifier.y)
        }.Select(pos => RotateVertexPoint(pos, origin, tr.Transform.Rotation));
    }
    
    private static SDL.SDL_FPoint RotateVertexPoint(Vector2 position, Vector2 pivot, Rotation rotation) {
        if (rotation == Rotation.Identity()) {
            return new SDL.SDL_FPoint { x = position.x, y = position.y };
        }

        Vector2 rotated = Vector2.RotatePointAroundPoint(position, pivot, rotation.Degree);
        return new SDL.SDL_FPoint { x = rotated.x, y = rotated.y };
    }
    
    private static void RenderBoldText(nint renderer, TextRenderer tr, Font font, Vector2 sizeModifier) {
        Vector2 direction = new Vector2(
            tr.Vertices[1].position.x - tr.Vertices[0].position.x,
            tr.Vertices[1].position.y - tr.Vertices[0].position.y
        ).Normalized * BOLD_OFFSET * sizeModifier;
        for (int i = 0; i < tr.Vertices.Length; i++) {
            SDL.SDL_FPoint p = tr.Vertices[i].position;
            tr.Vertices[i].position = new SDL.SDL_FPoint { x = p.x + direction.x, y = p.y + direction.y };
        }
        
        RenderTextGeometry(renderer, tr, font);
    }
    
    private static void RenderTextGeometry(IntPtr renderer, TextRenderer tr, Font font) {
        if (SDL.SDL_RenderGeometry(
            renderer,
            font.textureAtlas,
            tr.Vertices,
            tr.Vertices.Length,
            tr.Indices,
            tr.Indices.Length
        ) != 0) {
            throw new Exception($"Unable to render character due to: {SDL.SDL_GetError()}");
        }
    }
}