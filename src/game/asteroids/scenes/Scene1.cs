using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.animation.controller;
using Worms.engine.game_object.components.physics;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.engine.game_object.components.screen_pivot;
using Worms.engine.scene;
using Worms.game.asteroids.camera;
using Worms.game.asteroids.names;
using Worms.game.asteroids.player;

namespace Worms.game.asteroids.scenes; 

public static class Scene1 {
    public static Scene GetScene() {
        return new Scene("main", CreateWorldRoot, CreateScreenRoot);
    }
    
    private static GameObject CreateWorldRoot() {
        return GameObjectBuilder.Root()
            .Transform.AddChild("gameController")
            .SetLayer(LayerNames.PLAY_AREA_OBJECT)
            .SetComponent(new BoxCollider(true, ColliderState.TRIGGER, Vector2.One(), Vector2.Zero()))
            .SetComponent(new PlayArea())
            .Build()
            .Transform.AddSibling("player")
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateMultiple("player", 0, 0, 1, 2)).Build())
            .SetComponent(new PolygonCollider(true, PlayerMovement.COLLIDER_VERTICES, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new PlayerMovement())
            .Build()
                .Transform.AddChild("playAreaContainer")
                .SetLayer(LayerNames.PLAY_AREA_OBJECT)
                .SetComponent(new BoxCollider(true, ColliderState.TRIGGERING_COLLIDER, new Vector2(80, 80), Vector2.Zero()))
                .Build()
            .Transform.GetRoot().gameObject;
    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root()
            .Transform.AddChild("bottom-left")
            .SetComponent(new ScreenPivot(new Vector2Int(0, 0), true))
            .Build()
            .Transform.AddSibling("top-left")
            .SetComponent(new ScreenPivot(new Vector2Int(0, 1), true))
            .Build()
            .Transform.AddSibling("top-right")
            .SetComponent(new ScreenPivot(new Vector2Int(1, 1), true))
            .Build()
            .Transform.AddSibling("bottom-right")
            .SetComponent(new ScreenPivot(new Vector2Int(1, 0), true))
            .Build()
            .Transform.GetRoot().gameObject;
    }
}