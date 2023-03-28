namespace GameEngine.engine.window.menu;

public class WindowMenu : IMenuEntry {
    public readonly string text;
    public readonly List<IMenuEntry> menuItems;

    public WindowMenu(string text, List<IMenuEntry> menuItems) {
        this.text = text;
        this.menuItems = menuItems;
    }
}