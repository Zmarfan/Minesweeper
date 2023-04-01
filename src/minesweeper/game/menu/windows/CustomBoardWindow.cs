namespace Minesweeper.minesweeper.game.menu.windows;

public class CustomBoardWindow : PopupWindow {
    private const int CONTROL_Y_START_POS = 30;
    
    private const int BUTTON_X_POS = 133;
    private const int LABEL_X_POS = 15;
    private const int CONTROL_Y_SPACING = 25;
    
    private const int INPUT_X_POS = 65;

    public int BoardWidth => (int)_widthInput.Value;
    public int BoardHeight => (int)_heightInput.Value;
    public int BoardMines => (int)_minesInput.Value;
    
    private readonly NumericUpDown _widthInput;
    private readonly NumericUpDown _heightInput;
    private readonly NumericUpDown _minesInput;

    public CustomBoardWindow(BoardSettings boardSettings) : base("Custom Board", 225, 175) {
        _widthInput = CreateInput(boardSettings.width, 9, 75, CONTROL_Y_START_POS);
        CreateLabel("Width:", CONTROL_Y_START_POS);
        _heightInput = CreateInput(boardSettings.height, 9, 36, CONTROL_Y_START_POS + CONTROL_Y_SPACING);
        CreateLabel("Height:", CONTROL_Y_START_POS + CONTROL_Y_SPACING);
        _minesInput = CreateInput(boardSettings.mines, 1, 500, CONTROL_Y_START_POS + CONTROL_Y_SPACING * 2);
        CreateLabel("Mines:", CONTROL_Y_START_POS + CONTROL_Y_SPACING * 2);
        Controls.Add(WindowUtil.CreateButton("OK", BUTTON_X_POS, CONTROL_Y_START_POS, () => {
            DialogResult = DialogResult.OK;
            Close();
        }));
        Controls.Add(WindowUtil.CreateButton("Cancel", BUTTON_X_POS, CONTROL_Y_START_POS + CONTROL_Y_SPACING * 2, () => {
            DialogResult = DialogResult.Cancel;
            Close();
        }));
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
}