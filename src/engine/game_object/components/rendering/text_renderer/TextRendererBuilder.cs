using GameEngine.engine.core.renderer;
using GameEngine.engine.data;
using Color = GameEngine.engine.data.Color;

namespace GameEngine.engine.game_object.components.rendering.text_renderer; 

public class TextRendererBuilder {
    private bool _isActive = true;
    private string _name = "textRenderer";
    private string _sortingLayer = RendererHandler.DEFAULT_SORTING_LAYER;
    private int _sortOrder;
    private Color _color = Color.WHITE;
    private string _text = string.Empty;
    private readonly string _font;
    private int _width = 200;
    private int _size = 16;
    private bool _bold;
    private bool _italics;
    private TextAlignment _alignment = TextAlignment.LEFT;
    private int _lineSpacing;

    private TextRendererBuilder(string font) {
        _font = font;
    }

    public static TextRendererBuilder Builder(string font) {
        return new TextRendererBuilder(font);
    }

    public TextRenderer Build() {
        return new TextRenderer(_isActive, _name, _sortingLayer, _sortOrder, _color, _text, _font, _width, _size, _bold, _italics, _alignment, _lineSpacing);
    }
    
    public TextRendererBuilder SetIsActive(bool isActive) {
        _isActive = isActive;
        return this;
    }
    
    public TextRendererBuilder SetName(string name) {
        _name = name;
        return this;
    }
    
    public TextRendererBuilder SetSortingLayer(string sortingLayer) {
        _sortingLayer = sortingLayer;
        return this;
    }
    
    public TextRendererBuilder SetSortingOrder(int sortingOrder) {
        _sortOrder = sortingOrder;
        return this;
    }
    
    public TextRendererBuilder SetColor(Color color) {
        _color = color;
        return this;
    }
    
    public TextRendererBuilder SetText(string text) {
        _text = text;
        return this;
    }

    public TextRendererBuilder SetWidth(int width) {
        _width = width;
        return this;
    }
        
    public TextRendererBuilder SetSize(int size) {
        _size = size;
        return this;
    }
    
    public TextRendererBuilder SetBold() {
        _bold = true;
        return this;
    }
    
    public TextRendererBuilder SetItalics() {
        _italics = true;
        return this;
    }

    public TextRendererBuilder SetAlignment(TextAlignment alignment) {
        _alignment = alignment;
        return this;
    }

    public TextRendererBuilder SetLineSpacing(int spacing) {
        _lineSpacing = spacing;
        return this;
    }
}