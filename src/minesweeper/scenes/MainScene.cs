using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.scene;
using Minesweeper.minesweeper.game.board;
using Minesweeper.minesweeper.game.menu;
using Minesweeper.minesweeper.names;

namespace Minesweeper.minesweeper.scenes; 

public static class MainScene {
    public static Scene GetScene() {
        return new Scene(SceneNames.MAIN, CreateWorldRoot, CreateScreenRoot, camera => {
            camera.defaultDrawColor = new GameEngine.engine.data.Color(192, 192, 192, 255);
            camera.Size = 3.5f;
        });
    }

     private static GameObject CreateWorldRoot() {
        return GameObjectBuilder.Root()
            .Transform.AddChild("board")
            .SetLocalPosition(new Vector2(0, -Board.INFO_HEIGHT + Board.BORDER_LENGTH))
            .SetComponent(new MenuManager())
            .SetComponent(new Board(MenuManager.Settings))
            .Build()
            .Transform.GetRoot().gameObject;
    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root();
    }
}