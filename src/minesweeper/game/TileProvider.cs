using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.minesweeper.game.board;
using GameEngine.minesweeper.names;

namespace GameEngine.minesweeper.game; 

public static class TileProvider {
    public static Texture GetTexture(int mineCount, MarkType markType) {
        return markType switch {
            MarkType.NONE => CreateTileTexture(0, 0),
            MarkType.OPENED => mineCount switch {
                BoardCreator.BOMB => CreateTileTexture(5, 0),
                BoardCreator.OPENED_BOMB => CreateTileTexture(6, 0),
                BoardCreator.WRONG_BOMB => CreateTileTexture(7, 0),
                0 => CreateTileTexture(1, 0),
                _ => CreateTileTexture(mineCount - 1, 1)
            },
            MarkType.FLAGGED => CreateTileTexture(2, 0),
            MarkType.QUESTION_MARK => CreateTileTexture(3, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(markType), markType, null)
        };
    }

    private static Texture CreateTileTexture(int column, int row) {
        return Texture.CreateMultiple(TextureNames.TILES, column, row, 8, 2);
    }
}