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
    public int size;
    public bool bold;
    public bool italics;
    public bool underline;
    public bool strikeThrough;
    public TextAlignment alignment;
    public int characterSpacing;
    public int lineSpacing;
    public int wordSpacing;

    private string _text;
    private string _font;
    private int _width;

    private List<string>? _lines = null;
    
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
        bool underline,
        bool strikeThrough,
        TextAlignment alignment,
        int characterSpacing,
        int lineSpacing,
        int wordSpacing
    ) : base(isActive, sortingLayer, orderInLayer, color) {
        Text = text;
        Font = font;
        Width = width;
        this.size = size;
        this.bold = bold;
        this.italics = italics;
        this.underline = underline;
        this.strikeThrough = strikeThrough;
        this.alignment = alignment;
        this.characterSpacing = characterSpacing;
        this.lineSpacing = lineSpacing;
        this.wordSpacing = wordSpacing;
    }

    public List<string> GetLines(Font font) {
        if (_lines == null) {
            _lines = TextFormatter.FormatText(_text, _width, font);
        }

        return _lines;
    }
    
    public override void OnDrawGizmos() {
        Vector2 corner = Transform.LocalToWorldMatrix.ConvertPoint(new Vector2(Width, 0));
        Vector2 downVector = Transform.LocalToWorldMatrix.ConvertVector(Vector2.Down()).Normalized * 50;
        Gizmos.DrawLine(Transform.Position, corner, Color.RED);
        Gizmos.DrawRay(Transform.Position, downVector, Color.RED);
        Gizmos.DrawRay(corner, downVector, Color.RED);
    }
}