using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.audio_source;
using Worms.engine.game_object.components.physics.colliders;
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
            .SetComponent(AudioSourceBuilder.Builder(SoundNames.EXTRA_LIFE, ChannelNames.EFFECTS).SetPlayOnAwake(false).Build())
            .SetComponent(new GameController())
            .Build()
            .Transform.GetRoot().gameObject;
    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root();
    }
}