using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.animation.animation;
using Worms.engine.game_object.components.animation.composition;
using Worms.engine.game_object.components.animation.controller;
using Worms.engine.game_object.components.audio_source;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.engine.helper;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.player; 

public static class PlayerFactory {
    public static Transform Create(Transform parent) {
        Texture playerBase = Texture.CreateMultiple(TextureNames.PLAYER, 0, 0, 1, 2);
        Texture playerThrust = Texture.CreateMultiple(TextureNames.PLAYER, 0, 1, 1, 2);
        
        GameObject obj = parent.AddChild("player")
            .SetLayer(LayerNames.PLAYER)
            .SetTag(TagNames.PLAYER)
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateMultiple("player", 0, 0, 1, 2)).Build())
            .SetComponent(new PolygonCollider(true, PlayerBase.COLLIDER_VERTICES, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new PlayerBase())
            .SetComponent(new PlayerMovement())
            .SetComponent(AudioSourceBuilder
                .Builder(SoundNames.THRUST, ChannelNames.EFFECTS)
                .SetSourceName(PlayerMovement.THRUST_AUDIO_SOURCE)
                .SetPlayOnAwake(false)
                .Build()
            )
            .SetComponent(AudioSourceBuilder
                .Builder(SoundNames.FIRE, ChannelNames.EFFECTS)
                .SetSourceName(PlayerMovement.FIRE_AUDIO_SOURCE)
                .SetPlayOnAwake(false)
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
                .Transform.AddSibling("playerKillCollider")
                .SetLayer(LayerNames.PLAYER)
                .SetTag(TagNames.PLAYER)
                .SetComponent(new PolygonCollider(true, PlayerBase.COLLIDER_VERTICES, ColliderState.TRIGGERING_COLLIDER, Vector2.Zero()))
                .Build()
            .Transform.Parent!.gameObject;
        
        Transform.Instantiate(obj);
        return obj.Transform;
    }
}