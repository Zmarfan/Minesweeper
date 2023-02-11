using Worms.engine.data;

namespace Worms.engine.game_object.components.rendering.text_renderer; 

public class TextRenderer : RenderComponent {
    public string text;
    public string font;
    public int width;
    public int size;
    public bool bold;
    public bool italics;
    public bool underline;
    public bool strikeThrough;
    public TextAlignment alignment;
    public int characterSpacing;
    public int lineSpacing;
    public int wordSpacing;
    
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
        this.text = text;
        this.font = font;
        this.width = width;
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
}