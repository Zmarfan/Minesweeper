using Worms.engine.data;
using Worms.engine.game_object.components.particle_system.renderer;
using Worms.engine.game_object.components.texture_renderer;

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
        Renderer renderer
    ) {
        return GameObjectBuilder
            .Builder("particle")
            .SetPosition(startPosition)
            .SetScale(startSize)
            .SetRotation(startRotation)
            .SetComponent(new ParticleScript(localSpace, lifeTime, rotationVelocity, moveDirection, force))
            .SetComponent(new TextureRenderer(true, renderer.texture, renderer.sortingLayer, renderer.orderInLayer, Color.WHITE, renderer.flipX, renderer.flipY));
    }
}