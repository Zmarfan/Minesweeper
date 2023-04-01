using SDL2;
using GameEngine.engine.camera;
using GameEngine.engine.core.renderer.font;
using GameEngine.engine.data;
using GameEngine.engine.game_object.components;
using GameEngine.engine.game_object.components.rendering.text_renderer;
using Font = GameEngine.engine.core.renderer.font.Font;
using TextRenderer = GameEngine.engine.game_object.components.rendering.text_renderer.TextRenderer;

namespace GameEngine.engine.core.renderer; 

internal static class TextRendererHandler {
    private const int ITALICS_OFFSET = 20;
    private const int BOLD_OFFSET = 3;
    
    public static void RenderText(nint renderer, Font font, TextRenderer tr, TransformationMatrix matrix) {
        tr.RefreshDataIfNeeded(font);

        Vector2 scaleModifier = 1 / Camera.Main.Size * tr.Transform.Scale;
        float width = scaleModifier.x * tr.Width;
        Vector2 fontSizeModifier = scaleModifier * tr.Size / Font.FONT_SIZE;
        UpdateVertexPositions(tr, fontSizeModifier, font, matrix.ConvertPoint(tr.Transform.Position), width);

        RenderTextGeometry(renderer, tr, font);
        if (tr.bold) {
            RenderBoldText(renderer, tr, font, fontSizeModifier);
        }
    }

    private static void UpdateVertexPositions(
        TextRenderer tr,
        Vector2 sizeModifier,
        Font font,
        Vector2 origin,
        float width
    ) {
        Vector2 drawPosition = origin;
        
        int vertexIndex = 0;
        foreach (TextLine line in tr.Lines) {
            char? previous = null;
            if (line.text != string.Empty) {
                drawPosition.x = CalculateDrawStartPosition(origin.x, line.fraction, width, tr.alignment);
                foreach (char c in line.text) {
                    CharacterInfo info = font.characters[c];
                    float kerningOffset = (!previous.HasValue ? 0 : font.characters[c].kerningByCharacter[previous.Value]) * sizeModifier.x;
                    drawPosition.x += kerningOffset;
                    CalculateVertexPositions(drawPosition, info, font, sizeModifier, tr, origin, ref vertexIndex);
                    drawPosition.x += info.dimension.x * sizeModifier.x;
                    previous = c;
                }
            }

            drawPosition.x = origin.x;
            drawPosition.y += (font.maxCharHeight + tr.lineSpacing) * sizeModifier.y;
        }
    }

    private static float CalculateDrawStartPosition(float originX, float fraction, float width, TextAlignment alignment) {
        return alignment switch {
            TextAlignment.LEFT => originX,
            TextAlignment.CENTER => originX + fraction * width / 2,
            TextAlignment.RIGHT => originX + fraction * width,
            _ => throw new Exception($"Text renderer handler do not support this alignment style: {alignment}")
        };
    }

    private static void CalculateVertexPositions(
        Vector2 topLeftPosition,
        CharacterInfo info,
        Font font,
        Vector2 sizeModifier,
        TextRenderer tr,
        Vector2 origin,
        ref int vertexIndex
    ) {
        float charMaxHeightDiff = (font.maxCharHeight - info.dimension.y) * sizeModifier.y;
        float italicsOffset = tr.italics ? ITALICS_OFFSET * sizeModifier.x : 0;
        tr.Vertices[vertexIndex++].position = RotateVertexPoint(
            new Vector2(topLeftPosition.x + italicsOffset, topLeftPosition.y + charMaxHeightDiff),
            origin,
            tr
        );
        tr.Vertices[vertexIndex++].position = RotateVertexPoint(
            new Vector2(topLeftPosition.x + info.dimension.x * sizeModifier.x + italicsOffset, topLeftPosition.y + charMaxHeightDiff),
            origin,
            tr
        );
        tr.Vertices[vertexIndex++].position = RotateVertexPoint(
            new Vector2(topLeftPosition.x, topLeftPosition.y + font.maxCharHeight * sizeModifier.y),
            origin, 
            tr
        );
        tr.Vertices[vertexIndex++].position = RotateVertexPoint(
            new Vector2(topLeftPosition.x + info.dimension.x * sizeModifier.x, topLeftPosition.y + font.maxCharHeight * sizeModifier.y),
            origin,
            tr
        );
    }
    
    private static SDL.SDL_FPoint RotateVertexPoint(Vector2 position, Vector2 pivot, Component tr) {
        if (tr.Transform.Rotation == Rotation.Identity()) {
            return new SDL.SDL_FPoint { x = position.x, y = position.y };
        }

        Vector2 rotated = Vector2.RotatePointAroundPoint(position, pivot, tr.Transform.Rotation.Degree);
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
    
    private static void RenderTextGeometry(nint renderer, TextRenderer tr, Font font) {
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