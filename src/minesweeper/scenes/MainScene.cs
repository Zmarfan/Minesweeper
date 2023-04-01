using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.scene;
using GameEngine.minesweeper.game;
using GameEngine.minesweeper.game.board;
using GameEngine.minesweeper.game.menu;
using GameEngine.minesweeper.names;
using Color = GameEngine.engine.data.Color;

namespace GameEngine.minesweeper.scenes; 

public static class MainScene {
    public static Scene GetScene() {
        return new Scene(SceneNames.MAIN, CreateWorldRoot, CreateScreenRoot, camera => {
            camera.defaultDrawColor = new Color(192, 192, 192, 255);
            camera.Size = 3.5f;
        });
    }

     private static GameObject CreateWorldRoot() {
        return GameObjectBuilder.Root()
            .Transform.AddChild("board")
            .SetLocalPosition(new Vector2(0, -Board.INFO_HEIGHT + Board.BORDER_LENGTH))
            .SetComponent(new MenuManager())
            .SetComponent(new Board(MenuManager.Settings.width, MenuManager.Settings.height, MenuManager.Settings.mines, MenuManager.Settings.gameType))
            .Build()
            .Transform.GetRoot().gameObject;
    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root();
    }
}