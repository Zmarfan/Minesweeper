using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.minesweeper.game.board;
using GameEngine.minesweeper.names;

namespace GameEngine.minesweeper.game; 

public static class TextureProvider {
    public static Texture GetBorderTexture(BorderType type) {
        return type switch {
            BorderType.HORIZONTAL => CreateBorderTexture(0, 0),
            BorderType.VERTICAL => CreateBorderTexture(1, 2),
            BorderType.BOTTOM_LEFT_CORNER => CreateBorderTexture(0, 3),
            BorderType.BOTTOM_RIGHT_CORNER => CreateBorderTexture(1, 3),
            BorderType.TOP_LEFT_CORNER => CreateBorderTexture(0, 1),
            BorderType.TOP_RIGHT_CORNER => CreateBorderTexture(1, 0),
            BorderType.LEFT_PIPE => CreateBorderTexture(0, 2),
            BorderType.RIGHT_PIPE => CreateBorderTexture(1, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
    public static Texture GetTileTexture(int mineCount, MarkType markType) {
        return markType switch {
            MarkType.NONE => CreateTileTexture(0, 0),
            MarkType.OPENED => mineCount switch {
                Board.BOMB => CreateTileTexture(3, 0),
                Board.OPENED_BOMB => CreateTileTexture(4, 0),
                Board.WRONG_BOMB => CreateTileTexture(5, 0),
                0 => CreateTileTexture(1, 0),
                _ => CreateTileTexture(mineCount - 1, 1)
            },
            MarkType.FLAGGED => CreateTileTexture(2, 0),
            MarkType.QUESTION_MARK => CreateTileTexture(6, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(markType), markType, null)
        };
    }

    private static Texture CreateTileTexture(int column, int row) {
        return Texture.CreateMultiple(TextureNames.TILES, column, row, 8, 2);
    }
    
    private static Texture CreateBorderTexture(int column, int row) {
        return Texture.CreateMultiple(TextureNames.BORDER, column, row, 2, 4);
    }
}