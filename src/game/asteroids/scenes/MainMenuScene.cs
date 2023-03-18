using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.text_renderer;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.engine.game_object.components.screen_pivot;
using Worms.engine.scene;
using Worms.game.asteroids.controller;
using Worms.game.asteroids.main_menu;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.scenes; 

public static class MainMenuScene {
    public static Scene GetScene() {
        return new Scene(SceneNames.MAIN_MENU, CreateWorldRoot, CreateScreenRoot);
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
            .SetComponent(new MainMenuAsteroids())
            .Build()
            .Transform.AddSibling("menuHolder")
            .SetComponent(new MainMenuController())
            .SetPosition(new Vector2(0, 200))
            .Build()
                .Transform.AddChild("logo")
                .SetLocalPosition(new Vector2(-400, 100))
                .SetComponent(TextRendererBuilder
                    .Builder(FontNames.TITLE)
                    .SetAlignment(TextAlignment.CENTER)
                    .SetColor(Color.WHITE)
                    .SetSize(150)
                    .SetWidth(800)
                    .SetText("ASTEROIDS")
                    .Build()
                )
                .Build()
                .Transform.AddSibling("play")
                .SetLocalPosition(new Vector2(-400, -150))
                .SetComponent(TextRendererBuilder
                    .Builder(FontNames.MAIN)
                    .SetName(MainMenuController.PLAY)
                    .SetAlignment(TextAlignment.CENTER)
                    .SetColor(Color.WHITE)
                    .SetSize(25)
                    .SetWidth(800)
                    .SetText("PLAY GAME")
                    .Build()
                )
                .Build()
                .Transform.AddSibling("highscores")
                .SetLocalPosition(new Vector2(-400, -250))
                .SetComponent(TextRendererBuilder
                    .Builder(FontNames.MAIN)
                    .SetName(MainMenuController.SCORE)
                    .SetAlignment(TextAlignment.CENTER)
                    .SetColor(Color.WHITE)
                    .SetSize(25)
                    .SetWidth(800)
                    .SetText("HIGH SCORES")
                    .Build()
                )
                .Build()
                .Transform.AddSibling("quit")
                .SetLocalPosition(new Vector2(-400, -350))
                .SetComponent(TextRendererBuilder
                    .Builder(FontNames.MAIN)
                    .SetName(MainMenuController.QUIT)
                    .SetAlignment(TextAlignment.CENTER)
                    .SetColor(Color.WHITE)
                    .SetSize(25)
                    .SetWidth(800)
                    .SetText("QUIT")
                    .Build()
                )
                .Build()
            .Transform.GetRoot().gameObject;
    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root();
    }
}