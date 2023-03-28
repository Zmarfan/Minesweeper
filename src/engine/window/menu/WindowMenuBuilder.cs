namespace GameEngine.engine.window.menu; 

public class WindowMenuBuilder {
    private readonly string _text;
    private readonly List<IMenuEntry> _menuItems = new();

    private WindowMenuBuilder(string text) {
        _text = text;
    }

    public static WindowMenuBuilder Builder(string text) {
        return new WindowMenuBuilder(text);
    }

    public WindowMenu Build() {
        return new WindowMenu(_text, _menuItems);
    }

    public WindowMenuBuilder AddBreak() {
        _menuItems.Add(new MenuBreak());
        return this;
    }
    
    public WindowMenuBuilder AddItem(MenuItem item) {
        _menuItems.Add(item);
        return this;
    }
    
    public WindowMenuBuilder AddDropDown(WindowMenu menu) {
        _menuItems.Add(menu);
        return this;
    }
}