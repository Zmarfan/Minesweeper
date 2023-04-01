namespace GameEngine.minesweeper.game.menu.windows; 

public static class WindowUtil {
    private const int BUTTON_WIDTH = 60;
    private const int BUTTON_HEIGHT = 25;

    private static readonly Color HOVER_BUTTON_BACK_COLOR = Color.FromArgb(255, 229, 241, 251);
    private static readonly Color BUTTON_BACK_COLOR = Color.FromArgb(255, 225, 225, 225);
    private static readonly Color BUTTON_BORDER_COLOR = Color.FromArgb(255, 173, 173, 173);
    private static readonly Color FOCUS_BUTTON_COLOR = Color.FromArgb(255, 0, 120, 215);
    
    public static Button CreateButton(string title, int xPosition, int yPosition, Action onClick) {
        Button button = new();
        button.Text = title;
        button.Location = new Point(xPosition, yPosition);
        button.FlatStyle = FlatStyle.Flat;
        button.BackColor = BUTTON_BACK_COLOR;
        button.Width = BUTTON_WIDTH;
        button.Height = BUTTON_HEIGHT;
        button.FlatAppearance.BorderColor = BUTTON_BORDER_COLOR;
        button.GotFocus += ButtonFocus;
        button.LostFocus += ButtonLostFocus;
        button.MouseEnter += ButtonMouseEnter;
        button.MouseLeave += ButtonMouseLeave;
        button.Click += (_, _) => onClick();

        return button;
    }

    private static void ButtonFocus(object? obj, EventArgs eventArgs) {
        if (obj == null) {
            return;
        }
            
        Button button = (Button)obj;
        button.FlatAppearance.BorderColor = FOCUS_BUTTON_COLOR;
        button.FlatAppearance.BorderSize = 2;
    }
    
    private static void ButtonLostFocus(object? obj, EventArgs eventArgs) {
        if (obj == null) {
            return;
        }
            
        Button button = (Button)obj;
        button.FlatAppearance.BorderColor = BUTTON_BORDER_COLOR;
        button.FlatAppearance.BorderSize = 1;
    }
    
    private static void ButtonMouseEnter(object? obj, EventArgs eventArgs) {
        if (obj == null) {
            return;
        }
            
        Button button = (Button)obj;
        button.BackColor = HOVER_BUTTON_BACK_COLOR;
        button.FlatAppearance.BorderColor = FOCUS_BUTTON_COLOR;
    }
    
    private static void ButtonMouseLeave(object? obj, EventArgs eventArgs) {
        if (obj == null) {
            return;
        }
            
        Button button = (Button)obj;
        button.BackColor = BUTTON_BACK_COLOR;
        if (!button.Focused) {
            button.FlatAppearance.BorderColor = BUTTON_BORDER_COLOR;
        }
    }
}