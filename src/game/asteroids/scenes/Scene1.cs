using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.animation.controller;
using Worms.engine.game_object.components.physics;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.engine.game_object.components.screen_pivot;
using Worms.engine.scene;
using Worms.game.asteroids.camera;
using Worms.game.asteroids.player;

namespace Worms.game.asteroids.scenes; 

public static class Scene1 {
    public static Scene GetScene() {
        return new Scene("main", CreateWorldRoot, CreateScreenRoot);
    }
    
    private static GameObject CreateWorldRoot() {
        return GameObjectBuilder.Root()
            .Transform.AddChild("gameController")
            .SetComponent(new CameraInit())
            .Build()
            .Transform.AddChild("player")
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateMultiple("player", 0, 0, 1, 2)).Build())
            .SetComponent(new RigidBody(true))
            .SetComponent(new PolygonCollider(true, PlayerMovement.COLLIDER_VERTICES, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new PlayerMovement())
            .Build()
            .Transform.GetRoot().gameObject;
    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root()
            .Transform.AddChild("bottom-left")
            .SetComponent(new ScreenPivot(new Vector2(0, 0), true))
            .Build()
            .Transform.AddSibling("top-left")
            .SetComponent(new ScreenPivot(new Vector2(0, 1), true))
            .Build()
            .Transform.AddSibling("top-right")
            .SetComponent(new ScreenPivot(new Vector2(1, 1), true))
            .Build()
            .Transform.AddSibling("bottom-right")
            .SetComponent(new ScreenPivot(new Vector2(1, 0), true))
            .Build()
            .Transform.GetRoot().gameObject;
    }
}