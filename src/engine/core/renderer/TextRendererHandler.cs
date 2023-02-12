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

    private static SDL.SDL_Vertex[] CreateTextVertices(Camera camera, Font font, TextRenderer tr, TransformationMatrix matrix) {
        Vector2 drawPosition = matrix.ConvertPoint(tr.Transform.Position);
        Vector2 origin = drawPosition;
        float sizeModifier = 1 / camera.Size * tr.size / Font.FONT_SIZE;
        SDL.SDL_Color color = new() { r = tr.color.Rbyte, g = tr.color.Gbyte, b = tr.color.Bbyte, a = tr.color.Abyte };

        StringBuilder line = new();
        float lineWidth = 0;
        StringBuilder word = new();
        float wordWidth = 0;
        List<SDL.SDL_Vertex> vertices = new();
        for (int i = 0; i < tr.text.Length; i++) {
            char c = font.characters.ContainsKey(tr.text[i]) || tr.text[i] == '\n' ? tr.text[i] : '.';
            
            if (c == '\n') {
                if (lineWidth + wordWidth >= tr.width * sizeModifier) {
                    
                    vertices.AddRange(CreateVerticesForLine(
                        line.ToString(),
                        drawPosition,
                        origin,
                        tr.Transform.Rotation,
                        sizeModifier,
                        color,
                        font
                    ));
                    
                    lineWidth = 0;
                    line.Clear();
                    drawPosition.y += font.maxCharHeight * sizeModifier;
                }
                else if (lineWidth != 0) {
                    line.Append(' ');
                }

                line.Append(word);
                
                vertices.AddRange(CreateVerticesForLine(
                    line.ToString(),
                    drawPosition,
                    origin,
                    tr.Transform.Rotation,
                    sizeModifier,
                    color,
                    font
                ));
                
                wordWidth = 0;
                lineWidth = 0;
                line.Clear();
                word.Clear();
                drawPosition.y += font.maxCharHeight * sizeModifier;
                continue;
            }

            CharacterInfo info = font.characters[c];
            if (c != ' ') {
                wordWidth += info.dimension.x * sizeModifier;
            }

            if (wordWidth >= tr.width * sizeModifier) {
                if (lineWidth != 0) {
                    vertices.AddRange(CreateVerticesForLine(
                        line.ToString(),
                        drawPosition,
                        origin,
                        tr.Transform.Rotation,
                        sizeModifier,
                        color,
                        font
                    ));

                    lineWidth = 0;
                    line.Clear();
                    drawPosition.y += font.maxCharHeight * sizeModifier;
                }
                
                vertices.AddRange(CreateVerticesForLine(
                    word.ToString(),
                    drawPosition,
                    origin,
                    tr.Transform.Rotation,
                    sizeModifier,
                    color,
                    font
                ));
                
                wordWidth = info.dimension.x * sizeModifier;
                word.Clear();
                word.Append(c);
                drawPosition.y += font.maxCharHeight * sizeModifier;
                continue;
            }

            if (c != ' ') {
                word.Append(c);
            }
            
            if (c == ' ' || i == tr.text.Length - 1) {
                if (lineWidth + wordWidth >= tr.width * sizeModifier) {
                    
                    vertices.AddRange(CreateVerticesForLine(
                        line.ToString(),
                        drawPosition,
                        origin,
                        tr.Transform.Rotation,
                        sizeModifier,
                        color,
                        font
                    ));
                    
                    lineWidth = 0;
                    line.Clear();
                    drawPosition.y += font.maxCharHeight * sizeModifier;
                }
                else if (lineWidth != 0) {
                    line.Append(' ');
                    lineWidth += font.characters[' '].dimension.x * sizeModifier;
                }

                line.Append(word);
                lineWidth += wordWidth;
                wordWidth = 0;
                word.Clear();
            }
        }

        vertices.AddRange(CreateVerticesForLine(
            line.ToString(),
            drawPosition,
            origin,
            tr.Transform.Rotation,
            sizeModifier,
            color,
            font
        ));
        
        return vertices.ToArray();
    }

    private static IEnumerable<SDL.SDL_Vertex> CreateVerticesForLine(
        string line,
        Vector2 drawPosition,
        Vector2 pivot,
        Rotation rotation,
        float sizeModifier,
        SDL.SDL_Color color,
        Font font
    ) {
        List<SDL.SDL_Vertex> vertices = new();
        foreach (char c in line) {
            CharacterInfo info = font.characters[c];

            vertices.AddRange(CreateCharacterVertices(
                CalculateVertexPositions(drawPosition, info, font, sizeModifier),
                color,
                info,
                pivot,
                rotation
            ));
            drawPosition.x += info.dimension.x * sizeModifier;
        }

        return vertices;
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