using Worms.engine.core.audio;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.animation.animation;
using Worms.engine.game_object.components.animation.composition;
using Worms.engine.game_object.components.animation.controller;
using Worms.engine.game_object.components.audio_source;
using Worms.engine.game_object.components.physics;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.engine.game_object.components.screen_pivot;
using Worms.engine.helper;
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
        Texture playerBase = Texture.CreateMultiple(TextureNames.PLAYER, 0, 0, 1, 2);
        Texture playerThrust = Texture.CreateMultiple(TextureNames.PLAYER, 0, 1, 1, 2);
    
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
            .SetComponent(AudioSourceBuilder
                .Builder(SoundNames.THRUST, ChannelNames.EFFECTS)
                .SetPlayOnAwake(false)
                .SetLoop(true)
                .Build()
            )
            .SetComponent(AnimationControllerBuilder
                .Builder()
                .AddAnimation(PlayerMovement.THRUST_ANIMATION_TRIGGER, new Animation(0.05f, true, ListUtils.Of(
                    new Composition(g => g.GetComponent<TextureRenderer>(), ListUtils.Of(
                        new State(c => ((TextureRenderer)c).texture = playerThrust, 1),
                        new State(c => ((TextureRenderer)c).texture = playerBase, 1)
                    ))
                )))
                .Build())
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