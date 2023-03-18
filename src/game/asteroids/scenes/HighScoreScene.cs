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

public static class HighScoreScene {
    public static Scene GetScene() {
        return new Scene(SceneNames.HIGH_SCORE, CreateWorldRoot, CreateScreenRoot);
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
            .Transform.AddSibling("highscore")
            .SetComponent(new HighScoreMenuController())
            .SetPosition(new Vector2(-400, 450))
            .SetComponent(TextRendererBuilder
                .Builder(FontNames.MAIN)
                .SetAlignment(TextAlignment.CENTER)
                .SetWidth(800)
                .SetColor(Color.WHITE)
                .SetSize(45)
                .SetText("HIGH SCORES")
                .Build()
            )
            .Build()
            .Transform.AddSibling("back")
            .SetPosition(new Vector2(-400, -350))
            .SetComponent(TextRendererBuilder
                .Builder(FontNames.MAIN)
                .SetAlignment(TextAlignment.CENTER)
                .SetWidth(800)
                .SetColor(Color.WHITE)
                .SetSize(25)
                .SetText("- BACK -")
                .Build()
            )
            .Build()
            .Transform.GetRoot().gameObject;
    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root();
    }
}