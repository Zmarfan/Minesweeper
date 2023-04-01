namespace GameEngine.minesweeper.game.menu.windows; 

public class BestTimesWindow : PopupWindow {
    private const int BUTTON_Y_POSITION = 100;
    private const int LABEL_START_Y_POSITION = 15;
    private const int LABEL_Y_POSITION_SPACING = 25;
    private const int TYPE_X_POSITION = 30;
    private const int SCORE_X_POSITION = 150;

    public BestTimesWindow(int beginnerTime, int intermediateTime, int expertTime) : base("Best times", 300, 175) {
        CreateLabel("Beginner:", TYPE_X_POSITION, LABEL_START_Y_POSITION);
        CreateLabel("Intermediate:", TYPE_X_POSITION, LABEL_START_Y_POSITION + LABEL_Y_POSITION_SPACING);
        CreateLabel("Expert:", TYPE_X_POSITION, LABEL_START_Y_POSITION + LABEL_Y_POSITION_SPACING * 2);
        
        Label beginnerTimeLabel = CreateLabel(FormatTime(beginnerTime), SCORE_X_POSITION, LABEL_START_Y_POSITION, false);
        Label intermediateTimeLabel = CreateLabel(FormatTime(intermediateTime), SCORE_X_POSITION, LABEL_START_Y_POSITION + LABEL_Y_POSITION_SPACING, false);
        Label expertTimeLabel = CreateLabel(FormatTime(expertTime), SCORE_X_POSITION, LABEL_START_Y_POSITION + LABEL_Y_POSITION_SPACING * 2, false);
        
        Controls.Add(WindowUtil.CreateButton("Reset", 50, BUTTON_Y_POSITION, () => {
            HighScoreManager.Reset();
            beginnerTimeLabel.Text = FormatTime(HighScoreManager.WORST_TIME);
            intermediateTimeLabel.Text = FormatTime(HighScoreManager.WORST_TIME);
            expertTimeLabel.Text = FormatTime(HighScoreManager.WORST_TIME);
        }));
        Controls.Add(WindowUtil.CreateButton("OK", 175, BUTTON_Y_POSITION, () => {
            DialogResult = DialogResult.OK;
            Close();
        }));
    }
    
    private Label CreateLabel(string title, int xPosition, int yPosition, bool alignLeft = true) {
        Label label = new();
        label.Text = title;
        label.Location = new Point(xPosition, yPosition);
        label.TextAlign = alignLeft ? ContentAlignment.TopLeft : ContentAlignment.TopRight;
        Controls.Add(label);
        return label;
    }

    private static string FormatTime(int time) {
        return $"{time} seconds";
    }
}