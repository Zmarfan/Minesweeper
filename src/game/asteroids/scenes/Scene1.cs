using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.screen_pivot;
using Worms.engine.scene;
using Worms.game.asteroids.controller;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.scenes; 

public static class Scene1 {
    public static Scene GetScene() {
        return new Scene("main", CreateWorldRoot, CreateScreenRoot);
    }
    
    private static GameObject CreateWorldRoot() {
        Vector2[] defaultVertices = { new(-1, -1), new(-1, 1), new(1, 1) };
        
        return GameObjectBuilder.Root()
            .Transform.AddChild("gameController")
            .SetLayer(LayerNames.PLAY_AREA_OBJECT)
            .SetComponent(new PolygonCollider(true, defaultVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new PolygonCollider(true, defaultVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new PolygonCollider(true, defaultVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new PolygonCollider(true, defaultVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new GameController())
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