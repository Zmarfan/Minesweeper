using GameEngine.engine.core.saving;

namespace Minesweeper.minesweeper.game.menu; 

public static class HighScoreManager {
    public const int WORST_TIME = 999;

    private static readonly Dictionary<GameType, string> FILE_BY_TYPE = new() {
        { GameType.BEGINNER, "beginner" },
        { GameType.INTERMEDIATE, "intermediate" },
        { GameType.EXPERT, "expert" },
    };

    public static int LoadTime(GameType gameType) {
        if (!SaveManager.HasData<int>(FILE_BY_TYPE[gameType])) {
            return WORST_TIME;
        }

        return SaveManager.Load<int>(FILE_BY_TYPE[gameType]);
    }
    
    public static void SaveIfBetter(int time, GameType gameType) {
        if (time > WORST_TIME || !FILE_BY_TYPE.ContainsKey(gameType)) {
            return;
        }
        SaveIfBetter(time, FILE_BY_TYPE[gameType]);
    }

    private static void SaveIfBetter(int time, string file) {
        if (!SaveManager.HasData<int>(file) || SaveManager.Load<int>(file) > time) {
            SaveManager.Save(file, time);
        }
    }

    public static void Reset() {
        SaveManager.Save(FILE_BY_TYPE[GameType.BEGINNER], WORST_TIME);
        SaveManager.Save(FILE_BY_TYPE[GameType.INTERMEDIATE], WORST_TIME);
        SaveManager.Save(FILE_BY_TYPE[GameType.EXPERT], WORST_TIME);
    }
}