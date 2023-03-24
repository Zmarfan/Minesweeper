using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.minesweeper.game.number_display;
using GameEngine.minesweeper.names;

namespace GameEngine.minesweeper.game.board; 

public static class BoardBackgroundFactory {
    private const float DISPLAY_SCALE_FACTOR = Board.INFO_HEIGHT / (float)Board.BORDER_LENGTH;
    private const float DISPLAY_OFFSET = (Board.INFO_HEIGHT + Board.BORDER_LENGTH) / 2f;
    private const float NUM_DISPLAY_OFFSET = 160;

    public static GameObject Create(Transform parent, int width, int height) {
        Vector2 scaleFactor = CalculateScaleFactor(width, height);
        Vector2 boardOffset = CalculateBoardOffset(width, height);

        GameObject obj = parent.AddChild("boardBackground")
            .Build()
                .Transform.AddChild("bottom")
                .SetLocalPosition(new Vector2(0, -boardOffset.y))
                .SetLocalScale(new Vector2(scaleFactor.x, 1))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetBorderTexture(BorderType.HORIZONTAL)).Build())
                .Build()
                .Transform.AddSibling("top")
                .SetLocalPosition(new Vector2(0, boardOffset.y))
                .SetLocalScale(new Vector2(scaleFactor.x, 1))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetBorderTexture(BorderType.HORIZONTAL)).Build())
                .Build()
                .Transform.AddSibling("left")
                .SetLocalPosition(new Vector2(-boardOffset.x, 0))
                .SetLocalScale(new Vector2(1, scaleFactor.y))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetBorderTexture(BorderType.VERTICAL)).Build())
                .Build()
                .Transform.AddSibling("right")
                .SetLocalPosition(new Vector2(boardOffset.x, 0))
                .SetLocalScale(new Vector2(1, scaleFactor.y))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetBorderTexture(BorderType.VERTICAL)).Build())
                .Build()
            
                .Transform.AddSibling("bottomLeftCorner")
                .SetLocalPosition(new Vector2(-boardOffset.x, -boardOffset.y))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetBorderTexture(BorderType.BOTTOM_LEFT_CORNER)).Build())
                .Build()
                .Transform.AddSibling("bottomRightCorner")
                .SetLocalPosition(new Vector2(boardOffset.x, -boardOffset.y))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetBorderTexture(BorderType.BOTTOM_RIGHT_CORNER)).Build())
                .Build()
                .Transform.AddSibling("LeftPipe")
                .SetLocalPosition(new Vector2(-boardOffset.x, boardOffset.y))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetBorderTexture(BorderType.LEFT_PIPE)).Build())
                .Build()
                .Transform.AddSibling("RightPipe")
                .SetLocalPosition(new Vector2(boardOffset.x, boardOffset.y))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetBorderTexture(BorderType.RIGHT_PIPE)).Build())
                .Build()
            
                .Transform.AddSibling("leftDisplay")
                .SetLocalScale(new Vector2(1, DISPLAY_SCALE_FACTOR))
                .SetLocalPosition(new Vector2(-boardOffset.x, boardOffset.y + DISPLAY_OFFSET))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetBorderTexture(BorderType.VERTICAL)).Build())
                .Build()
                .Transform.AddSibling("rightDisplay")
                .SetLocalScale(new Vector2(1, DISPLAY_SCALE_FACTOR))
                .SetLocalPosition(new Vector2(boardOffset.x, boardOffset.y + DISPLAY_OFFSET))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetBorderTexture(BorderType.VERTICAL)).Build())
                .Build()
                .Transform.AddSibling("topDisplay")
                .SetLocalScale(new Vector2(scaleFactor.x, 1))
                .SetLocalPosition(new Vector2(0, boardOffset.y + DISPLAY_OFFSET * 2))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetBorderTexture(BorderType.HORIZONTAL)).Build())
                .Build()
            
                .Transform.AddSibling("topLeftDisplayCorner")
                .SetLocalPosition(new Vector2(-boardOffset.x, boardOffset.y + DISPLAY_OFFSET * 2))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetBorderTexture(BorderType.TOP_LEFT_CORNER)).Build())
                .Build()
                .Transform.AddSibling("topRightDisplayCorner")
                .SetLocalPosition(new Vector2(boardOffset.x, boardOffset.y + DISPLAY_OFFSET * 2))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetBorderTexture(BorderType.TOP_RIGHT_CORNER)).Build())
                .Build()
            .Transform.Parent!.gameObject;
        NumberDisplayFactory.Create(obj.Transform, new Vector2(-boardOffset.x + NUM_DISPLAY_OFFSET, boardOffset.y + DISPLAY_OFFSET), Board.MINE_NUMBER_DISPLAY);
        NumberDisplayFactory.Create(obj.Transform, new Vector2(boardOffset.x - NUM_DISPLAY_OFFSET, boardOffset.y + DISPLAY_OFFSET), Board.TIME_NUMBER_DISPLAY);
        
        Transform.Instantiate(obj);
        return obj;
    }
    
    private static Vector2 CalculateScaleFactor(int width, int height) {
        return new Vector2(width, height) * Board.TILE_LENGTH / Board.BORDER_LENGTH;
    }
    
    private static Vector2 CalculateBoardOffset(int width, int height) {
        return (new Vector2(width, height) * Board.TILE_LENGTH + new Vector2(Board.BORDER_LENGTH, Board.BORDER_LENGTH)) / 2f;
    }
}