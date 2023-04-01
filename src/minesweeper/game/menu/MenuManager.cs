using GameEngine.engine.game_object.scripts;
using GameEngine.engine.scene;
using GameEngine.engine.window;
using GameEngine.minesweeper.names;

namespace GameEngine.minesweeper.game.menu; 

public class MenuManager : Script {
    private static readonly BoardSettings BEGINNER_SETTINGS = new(9, 9, 10);
    private static readonly BoardSettings INTERMEDIATE_SETTINGS = new(16, 16, 40);
    private static readonly BoardSettings EXPERT_SETTINGS = new(30, 16, 99);
    
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
            case MenuNames.EXIT:
                SceneManager.Quit();
                break;
        }
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
        WindowMenuHandler.ChangeMenuItemCheckStatus(MenuNames.MARKS, UseQuestionMarks);
    }
}