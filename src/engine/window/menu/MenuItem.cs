namespace GameEngine.engine.window.menu; 

public class MenuItem : IMenuEntry {
    public readonly string identifier;
    public readonly string text;
    public readonly bool isChecked;
    public readonly bool disabled;

    public MenuItem(string identifier, string text, bool isChecked, bool disabled) {
        this.identifier = identifier;
        this.text = text;
        this.isChecked = isChecked;
        this.disabled = disabled;
    }
}