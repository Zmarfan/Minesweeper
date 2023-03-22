using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.scene;
using GameEngine.minesweeper.game.board;
using GameEngine.minesweeper.names;

namespace GameEngine.minesweeper.scenes; 

public static class Scene1 {
    public static Scene GetScene() {
        return new Scene(SceneNames.MAIN, CreateWorldRoot, CreateScreenRoot, c => {
            c.defaultDrawColor = Color.WHITE;
        });
    }

     private static GameObject CreateWorldRoot() {
        return GameObjectBuilder.Root()
            .Transform.AddChild("board")
            .SetComponent(new Board(9, 9))
            .Build()
            .Transform.GetRoot().gameObject;
    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root();
    }
}