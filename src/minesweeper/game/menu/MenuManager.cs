using GameEngine.engine.core.saving;
using GameEngine.engine.game_object.scripts;
using GameEngine.engine.scene;
using GameEngine.engine.window;
using GameEngine.minesweeper.game.menu.windows;
using GameEngine.minesweeper.names;

namespace GameEngine.minesweeper.game.menu; 

public class MenuManager : Script {
    private static readonly BoardSettings BEGINNER_SETTINGS = new(9, 9, 10, GameType.BEGINNER);
    private static readonly BoardSettings INTERMEDIATE_SETTINGS = new(16, 16, 40, GameType.INTERMEDIATE);
    private static readonly BoardSettings EXPERT_SETTINGS = new(30, 16, 99, GameType.EXPERT);
    
    public static BoardSettings Settings { get; private set; } = BEGINNER_SETTINGS;
    public static bool UseQuestionMarks { get; private set; } = true;

    public MenuManager() {
        WindowMenuHandler.MenuItemClicked += MenuItemClicked;
        SetMenuCheckStatus();
    }

    public override void OnDestroy() {
        WindowMenuHandler.MenuItemClicked -= MenuItemClicked;    }

    private static void MenuItemClicked(string identifier) {
        switch (identifier) {
            case MenuNames.NEW:
                SceneManager.LoadScene(SceneNames.MAIN);
                break;
            case MenuNames.BEGINNER:
                RestartGame(BEGINNER_SETTINGS);
                break;
            case MenuNames.INTERMEDIATE:
                RestartGame(INTERMEDIATE_SETTINGS);
                break;
            case MenuNames.EXPERT:
                RestartGame(EXPERT_SETTINGS);
                break;
            case MenuNames.MARKS:
                UseQuestionMarks = !UseQuestionMarks;
                WindowMenuHandler.ChangeMenuItemCheckStatus(MenuNames.MARKS, UseQuestionMarks);
                break;
            case MenuNames.CUSTOM:
                OpenCustomWindow();
                break;
            case MenuNames.BEST_TIMES:
                OpenBestTimesWindow();
                break;
            case MenuNames.EXIT:
                SceneManager.Quit();
                break;
        }
    }
    
    private static void OpenCustomWindow() {
        CustomBoardWindow dialog = new(Settings);
        if (dialog.ShowDialog() == DialogResult.OK) {
            if (IsCustomSameAsPreset(dialog, BEGINNER_SETTINGS)) {
                RestartGame(BEGINNER_SETTINGS);
            }
            else if (IsCustomSameAsPreset(dialog, INTERMEDIATE_SETTINGS)) {
                RestartGame(INTERMEDIATE_SETTINGS);
            }
            else if (IsCustomSameAsPreset(dialog, EXPERT_SETTINGS)) {
                RestartGame(EXPERT_SETTINGS);
            }
            else {
                RestartGame(new BoardSettings(
                    dialog.BoardWidth,
                    dialog.BoardHeight,
                    Math.Min(dialog.BoardMines, dialog.BoardWidth * dialog.BoardHeight),
                    GameType.CUSTOM
                ));
            }
        }
        SetMenuCheckStatus();
    }

    private static bool IsCustomSameAsPreset(CustomBoardWindow dialog, BoardSettings settings) {
        return dialog.BoardWidth == settings.width && dialog.BoardHeight == settings.height && dialog.BoardMines == settings.mines;
    }

    private static void OpenBestTimesWindow() {
        BestTimesWindow dialog = new(
            HighScoreManager.LoadTime(GameType.BEGINNER), 
            HighScoreManager.LoadTime(GameType.INTERMEDIATE), 
            HighScoreManager.LoadTime(GameType.EXPERT)
        );
        dialog.ShowDialog();
    }
    
    private static void RestartGame(BoardSettings settings) {
        Settings = settings;
        SetMenuCheckStatus();
        SceneManager.LoadScene(SceneNames.MAIN);
    }

    private static void SetMenuCheckStatus() {
        WindowMenuHandler.ChangeMenuItemCheckStatus(MenuNames.BEGINNER, Settings == BEGINNER_SETTINGS);
        WindowMenuHandler.ChangeMenuItemCheckStatus(MenuNames.INTERMEDIATE, Settings == INTERMEDIATE_SETTINGS);
        WindowMenuHandler.ChangeMenuItemCheckStatus(MenuNames.EXPERT, Settings == EXPERT_SETTINGS);
        WindowMenuHandler.ChangeMenuItemCheckStatus(MenuNames.CUSTOM, Settings != BEGINNER_SETTINGS && Settings != INTERMEDIATE_SETTINGS && Settings != EXPERT_SETTINGS);
        WindowMenuHandler.ChangeMenuItemCheckStatus(MenuNames.MARKS, UseQuestionMarks);
    }
}