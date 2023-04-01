namespace GameEngine.minesweeper.game.menu.windows;

public class CustomBoardWindow : Form {
    private const int CONTROL_Y_START_POS = 30;
    
    private const int LABEL_X_POS = 15;
    private const int CONTROL_Y_SPACING = 25;
    
    private const int INPUT_X_POS = 65;
    private const int BUTTON_X_POS = 133;

    private const int BUTTON_WIDTH = 60;
    private const int BUTTON_HEIGHT = 25;

    private static readonly Color HOVER_BUTTON_BACK_COLOR = Color.FromArgb(255, 229, 241, 251);
    private static readonly Color BUTTON_BACK_COLOR = Color.FromArgb(255, 225, 225, 225);
    private static readonly Color BUTTON_BORDER_COLOR = Color.FromArgb(255, 173, 173, 173);
    private static readonly Color FOCUS_BUTTON_COLOR = Color.FromArgb(255, 0, 120, 215);

    public int BoardWidth => (int)_widthInput.Value;
    public int BoardHeight => (int)_heightInput.Value;
    public int BoardMines => (int)_minesInput.Value;
    
    private readonly NumericUpDown _widthInput;
    private readonly NumericUpDown _heightInput;
    private readonly NumericUpDown _minesInput;

    public CustomBoardWindow(BoardSettings boardSettings) {
        Text = "Custom Board";
        Icon = new Icon(Minesweeper.Path("icon.ico"));
        Size = new Size(225, 175);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        _widthInput = CreateInput(boardSettings.width, 9, 75, CONTROL_Y_START_POS);
        CreateLabel("Width:", CONTROL_Y_START_POS);
        _heightInput = CreateInput(boardSettings.height, 9, 36, CONTROL_Y_START_POS + CONTROL_Y_SPACING);
        CreateLabel("Height:", CONTROL_Y_START_POS + CONTROL_Y_SPACING);
        _minesInput = CreateInput(boardSettings.mines, 1, 500, CONTROL_Y_START_POS + CONTROL_Y_SPACING * 2);
        CreateLabel("Mines:", CONTROL_Y_START_POS + CONTROL_Y_SPACING * 2);
        CreateButton("OK", CONTROL_Y_START_POS, () => {
            DialogResult = DialogResult.OK;
            Close();
        });
        CreateButton("Cancel", CONTROL_Y_START_POS + CONTROL_Y_SPACING * 2, () => {
            DialogResult = DialogResult.Cancel;
            Close();
        });
    }
    
    private NumericUpDown CreateInput(int defaultValue, int min, int max, int yPosition) {
        NumericUpDown input = new();
        input.Location = new Point(INPUT_X_POS, yPosition);
        input.MaximumSize = new Size(50, 25);
        input.Minimum = min;
        input.Maximum = max;
        input.Value = defaultValue;
        Controls.Add(input);
        return input;
    }
    
    private void CreateLabel(string title, int yPosition) {
        Label label = new();
        label.Text = title;
        label.Location = new Point(LABEL_X_POS, yPosition + 2);
        Controls.Add(label);
    }

    private void CreateButton(string title, int yPosition, Action onClick) {
        Button button = new();
        button.Text = title;
        button.Location = new Point(BUTTON_X_POS, yPosition);
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
        
        Controls.Add(button);
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