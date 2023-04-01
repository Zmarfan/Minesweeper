namespace GameEngine.minesweeper.game.menu; 

public record BoardSettings(int width, int height, int mines) {
    public readonly int width = width;
    public readonly int height = height;
    public readonly int mines = mines;
}