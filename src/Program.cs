using GameEngine.engine.core;

namespace Minesweeper; 

public static class Program {
    public static void Main() {
        Game game = minesweeper.Minesweeper.CreateGame();
        game.Run();
    }
}