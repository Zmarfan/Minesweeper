using GameEngine.engine.core;
using GameEngine.minesweeper;

namespace GameEngine; 

public static class Program {
    public static void Main() {
        Game game = Minesweeper.CreateGame();
        game.Run();
    }
}