using Worms.engine.core.renderer;
using Worms.engine.data;
using Worms.engine.game_object.components.rendering.texture_renderer;

namespace Worms.engine.game_object.components.rendering.text_renderer; 

public class TextRendererBuilder {
    private bool _isActive = true;
    private string _sortingLayer = RendererHandler.DEFAULT_SORTING_LAYER;
    private int _sortOrder = 0;
    private Color _color = Color.WHITE;
    private string _text = string.Empty;
    private string _font = string.Empty;
    private int _width = 200;
    private int _size = 16;
    private bool _bold = false;
    private bool _italics = false;
    private TextAlignment _alignment = TextAlignment.LEFT;
    private int _characterSpacing = 0;
    private int _lineSpacing = 0;
    private int _wordSpacing = 0;

    public static TextRendererBuilder Builder() {
        return new TextRendererBuilder();
    }

    public TextRenderer Build() {
        return new TextRenderer(_isActive, _sortingLayer, _sortOrder, _color, _text, _font, _width, _size, _bold, _italics, _alignment, _characterSpacing, _lineSpacing, _wordSpacing);
    }
    
    public TextRendererBuilder SetIsActive(bool isActive) {
        _isActive = isActive;
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
    
    public TextRendererBuilder SetFont(string font) {
        _font = font;
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
    
    public TextRendererBuilder SetCharacterSpacing(int spacing) {
        _characterSpacing = spacing;
        return this;
    }
    
    public TextRendererBuilder SetLineSpacing(int spacing) {
        _lineSpacing = spacing;
        return this;
    }
    
    public TextRendererBuilder SetWordSpacing(int spacing) {
        _wordSpacing = spacing;
        return this;
    }
}