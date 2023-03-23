using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.scene;
using GameEngine.minesweeper.game.board;
using GameEngine.minesweeper.names;

namespace GameEngine.minesweeper.scenes; 

public static class Scene1 {
    public static Scene GetScene() {
        return new Scene(SceneNames.MAIN, CreateWorldRoot, CreateScreenRoot, camera => {
            camera.defaultDrawColor = Color.WHITE;
            camera.Size = 3.5f;
        });
    }

     private static GameObject CreateWorldRoot() {
        return GameObjectBuilder.Root()
            .Transform.AddChild("board")
            .SetComponent(new Board(9, 9, 10))
            .Build()
            .Transform.GetRoot().gameObject;
    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root();
    }
}