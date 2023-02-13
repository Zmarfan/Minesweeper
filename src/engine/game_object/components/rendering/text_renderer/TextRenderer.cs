using SDL2;
using Worms.engine.core.gizmos;
using Worms.engine.core.renderer.font;
using Worms.engine.data;

namespace Worms.engine.game_object.components.rendering.text_renderer; 

public class TextRenderer : RenderComponent {
    public string Text {
        get => _text;
        set {
            if (_text == value) {
                return;
            }

            _lines = null;
            _text = value;
        }
    }
    public string Font {
        get => _font;
        set {
            if (_font == value) {
                return;
            }

            _lines = null;
            _font = value;
        }
    }
    public int Width {
        get => _width;
        set {
            if (_width == value) {
                return;
            }

            _lines = null;
            _width = value;
        }
    }
    public int Size {
        get => _size;
        set {
            if (_size == value) {
                return;
            }

            _lines = null;
            _size = value;
        }
    }
    public bool bold;
    public bool italics;
    public TextAlignment alignment;
    public int characterSpacing;
    public int lineSpacing;
    public int wordSpacing;

    public IEnumerable<string> Lines => _lines!;

    private string _text;
    private string _font;
    private int _width;
    private int _size;

    private List<string>? _lines = null;
    public SDL.SDL_Vertex[] Vertices { get; private set; } = null!;
    public int[] Indices { get; private set; } = null!;
    
    public TextRenderer(
        bool isActive,
        string sortingLayer,
        int orderInLayer,
        Color color,
        string text,
        string font,
        int width,
        int size,
        bool bold,
        bool italics,
        TextAlignment alignment,
        int characterSpacing,
        int lineSpacing,
        int wordSpacing
    ) : base(isActive, sortingLayer, orderInLayer, color) {
        _text = text;
        _font = font;
        _width = width;
        _size = size;
        this.bold = bold;
        this.italics = italics;
        this.alignment = alignment;
        this.characterSpacing = characterSpacing;
        this.lineSpacing = lineSpacing;
        this.wordSpacing = wordSpacing;
    }

    public void RefreshDataIfNeeded(Font font) {
        if (_lines == null) {
            _lines = TextFormatter.FormatText(_text, _width, _size, font);
            Vertices = CreateVertices(font);
            Indices = CreateIndices();
        }
    }

    private SDL.SDL_Vertex[] CreateVertices(Font font) {
        List<string> lines = _lines!;
        SDL.SDL_Color vertexColor = new() { r = color.Rbyte, g = color.Gbyte, b = color.Bbyte, a = color.Abyte };

        List<SDL.SDL_Vertex> vertices = new();
        foreach (string line in lines.Where(line => line != string.Empty)) {
            vertices.AddRange(CreateVerticesForLine(line, vertexColor, font));
        }
        
        return vertices.ToArray();
    }
    
    private static IEnumerable<SDL.SDL_Vertex> CreateVerticesForLine(string line, SDL.SDL_Color color, Font font) {
        return line
            .Select(c => font.characters[c])
            .SelectMany(info => new [] {
                new SDL.SDL_Vertex { color = color, tex_coord = info.textureCoords[0] },
                new SDL.SDL_Vertex { color = color, tex_coord = info.textureCoords[1] },
                new SDL.SDL_Vertex { color = color, tex_coord = info.textureCoords[2] },
                new SDL.SDL_Vertex { color = color, tex_coord = info.textureCoords[3] }
            });
    }

    private int[] CreateIndices() {
        int totalCharacters = Vertices.Length / 4;
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
    
    public override void OnDrawGizmos() {
        Vector2 corner = Transform.LocalToWorldMatrix.ConvertPoint(new Vector2(Width, 0));
        Vector2 downVector = Transform.LocalToWorldMatrix.ConvertVector(Vector2.Down()).Normalized * 50;
        Gizmos.DrawLine(Transform.Position, corner, Color.RED);
        Gizmos.DrawRay(Transform.Position, downVector, Color.RED);
        Gizmos.DrawRay(corner, downVector, Color.RED);
    }
}