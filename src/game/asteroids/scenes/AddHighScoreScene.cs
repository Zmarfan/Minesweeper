using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.text_renderer;
using Worms.engine.scene;
using Worms.game.asteroids.controller;
using Worms.game.asteroids.menu;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.scenes; 

public static class AddHighScoreScene {
    public static Scene GetScene() {
        return new Scene(SceneNames.ADD_HIGH_SCORE, CreateWorldRoot, CreateScreenRoot);
    }
    
    private static GameObject CreateWorldRoot() {
        Vector2[] defaultVertices = { new(-1, -1), new(-1, 1), new(1, 1) };
        
        return GameObjectBuilder.Root()
            .Transform.AddChild("screenContainer")
            .SetLayer(LayerNames.PLAY_AREA_OBJECT)
            .SetComponent(new PolygonCollider(true, defaultVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new PolygonCollider(true, defaultVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new PolygonCollider(true, defaultVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new PolygonCollider(true, defaultVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new ScreenContainer())
            .SetComponent(new MenuAsteroidsSpawner())
            .Build()
            .Transform.AddSibling("explaining")
            .SetLocalPosition(new Vector2(-400, 300))
            .SetComponent(TextRendererBuilder
                .Builder(FontNames.MAIN)
                .SetAlignment(TextAlignment.CENTER)
                .SetWidth(800)
                .SetColor(Color.WHITE)
                .SetSize(25)
                .SetText($"YOUR SCORE OF {EnterHighScore.SCORE} IS ONE OF THE TEN BEST\nPLEASE ENTER YOUR INITIALS")
                .Build()
            )
            .Build()
            .Transform.AddSibling("textEnter")
            .SetLocalPosition(new Vector2(-400, 0))
            .SetComponent(new EnterHighScore())
            .SetComponent(TextRendererBuilder
                .Builder(FontNames.MAIN)
                .SetAlignment(TextAlignment.CENTER)
                .SetWidth(800)
                .SetColor(Color.WHITE)
                .SetSize(60)
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