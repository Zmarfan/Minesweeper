namespace GameEngine.engine.window.menu; 

public class MenuItemBuilder {
    private readonly string _identifier;
    private readonly string _text;
    private bool _isChecked;
    private bool _disabled;

    private MenuItemBuilder(string identifier, string text) {
        _identifier = identifier;
        _text = text;
    }

    public static MenuItemBuilder Builder(string identifier, string text) {
        return new MenuItemBuilder(identifier, text);
    }

    public MenuItem Build() {
        return new MenuItem(_identifier, _text, _isChecked, _disabled);
    }

    public MenuItemBuilder IsChecked(bool isChecked) {
        _isChecked = isChecked;
        return this;
    }

    public MenuItemBuilder IsDisabled(bool isDisabled) {
        _disabled = isDisabled;
        return this;
    }
}