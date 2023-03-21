using GameEngine.engine.game_object;
using GameEngine.engine.scene;
using GameEngine.minesweeper.names;

namespace GameEngine.minesweeper.scenes; 

public class Scene1 {
     public static Scene GetScene() {
        return new Scene(SceneNames.MAIN, CreateWorldRoot, CreateScreenRoot);
    }
    
    private static GameObject CreateWorldRoot() {
        return GameObjectBuilder.Root().Transform.GetRoot().gameObject;
    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root();
    }
}