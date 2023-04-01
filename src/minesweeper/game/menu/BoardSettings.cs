namespace GameEngine.minesweeper.game.menu; 

public record BoardSettings(int width, int height, int mines, GameType gameType) {
    public readonly int width = width;
    public readonly int height = height;
    public readonly int mines = mines;
    public readonly GameType gameType = gameType;
}