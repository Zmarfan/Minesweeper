using Worms.engine.data;
using Worms.engine.game_object.components.animation.animation;
using Worms.engine.game_object.components.animation.controller;
using Worms.engine.game_object.components.particle_system.renderer;
using Worms.engine.game_object.components.rendering.texture_renderer;

namespace Worms.engine.game_object.components.particle_system.particle; 

public static class ParticleFactory {
    public static GameObjectBuilder ParticleBuilder(
        bool localSpace,
        Vector2 startPosition,
        Vector2 startSize,
        Rotation startRotation,
        float lifeTime,
        Vector2 moveDirection,
        float rotationVelocity,
        Vector2 force,
        ParticleSystemBuilder? subSystem,
        Func<Animation>? animationProvider,
        Renderer renderer
    ) {
        GameObjectBuilder builder = GameObjectBuilder
            .Builder("particle")
            .SetLocalPosition(startPosition)
            .SetLocalScale(startSize)
            .SetLocalRotation(startRotation)
            .SetComponent(new ParticleScript(localSpace, lifeTime, rotationVelocity, moveDirection, force))
            .SetComponent(new TextureRenderer(true, renderer.texture, renderer.sortingLayer, renderer.orderInLayer, Color.WHITE, renderer.flipX, renderer.flipY));

        if (subSystem != null) {
            builder.SetComponent(subSystem.Build());
        }

        if (animationProvider != null) {
            builder.SetComponent(AnimationControllerBuilder
                .Builder()
                .AddAnimation("particleAnimation", animationProvider.Invoke())
                .SetStartAnimation(0)
                .Build()
            );
        }
        
        return builder;
    }
}