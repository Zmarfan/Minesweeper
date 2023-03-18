using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.text_renderer;
using Worms.engine.scene;
using Worms.game.asteroids.controller;
using Worms.game.asteroids.main_menu;
using Worms.game.asteroids.menu;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.scenes; 

public static class AddHighScoreScene {
    public static Scene GetScene() {
        return new Scene(SceneNames.ADD_HIGH_SCORE, CreateWorldRoot, CreateScreenRoot);
    }
    
    private static GameObject CreateWorldRoot() {
        return GameObjectBuilder.Root()
            .Transform.AddChild("textEnter")
            .SetLocalPosition(new Vector2(-400, 0))
            .SetComponent(new EnterHighScore())
            .SetComponent(TextRendererBuilder
                .Builder(FontNames.MAIN)
                .SetAlignment(TextAlignment.CENTER)
                .SetWidth(800)
                .SetColor(Color.WHITE)
                .SetSize(45)
                .SetText("A__")
                .Build()
            )
            .Build()
            .Transform.Parent!.gameObject;

    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root();
    }
}