using SDL2;
using GameEngine.engine.core.gizmos;
using GameEngine.engine.core.renderer.font;
using GameEngine.engine.data;

namespace GameEngine.engine.game_object.components.rendering.text_renderer; 

public class TextRenderer : RenderComponent {
    public override Color Color {
        get => color;
        set {
            if (color == value) {
                return;
            }

            _lines = null;
            color = value;
        }
    }

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
    public int lineSpacing;

    public IEnumerable<TextLine> Lines => _lines!;

    private string _text;
    private string _font;
    private int _width;
    private int _size;

    private List<TextLine>? _lines;
    public SDL.SDL_Vertex[] Vertices { get; private set; } = null!;
    public int[] Indices { get; private set; } = null!;
    
    public TextRenderer(
        bool isActive,
        string name,
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
        int lineSpacing
    ) : base(sortingLayer, orderInLayer, color, isActive, name) {
        _text = text;
        _font = font;
        _width = width;
        _size = size;
        this.bold = bold;
        this.italics = italics;
        this.alignment = alignment;
        this.lineSpacing = lineSpacing;
    }

    public void RefreshDataIfNeeded(Font font) {
        if (_lines == null) {
            _lines = TextFormatter.FormatText(_text, _width, _size, font);
            Vertices = CreateVertices(font);
            Indices = CreateIndices();
        }
    }

    private SDL.SDL_Vertex[] CreateVertices(Font font) {
        List<TextLine> lines = _lines!;
        SDL.SDL_Color vertexColor = new() { r = Color.Rbyte, g = Color.Gbyte, b = Color.Bbyte, a = Color.Abyte };

        List<SDL.SDL_Vertex> vertices = new();
        foreach (TextLine line in lines.Where(line => line.text != string.Empty)) {
            vertices.AddRange(CreateVerticesForLine(line.text, vertexColor, font));
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
        Gizmos.DrawLine(Transform.Position, corner, GizmoSettings.TEXT_AREA_NAME);
        Gizmos.DrawRay(Transform.Position, downVector, GizmoSettings.TEXT_AREA_NAME);
        Gizmos.DrawRay(corner, downVector, GizmoSettings.TEXT_AREA_NAME);
    }
}