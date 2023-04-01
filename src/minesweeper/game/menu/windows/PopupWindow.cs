namespace GameEngine.minesweeper.game.menu.windows; 

public class PopupWindow : Form {
    protected PopupWindow(string title, int width, int height) {
        Size = new Size(width, height);
        Text = title;
        Icon = new Icon(Minesweeper.Path("icon.ico"));
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
    }
}