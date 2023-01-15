using Worms.engine.data;
using Worms.engine.game_object.components.particle_system.renderer;
using Worms.engine.game_object.components.texture_renderer;

namespace Worms.engine.game_object.components.particle_system.particle; 

public static class ParticleFactory {
    public static GameObjectBuilder ParticleBuilder(
        Vector2 startPosition,
        Vector2 startSize,
        Rotation startRotation,
        float lifeTime,
        Vector2 moveDirection,
        Renderer renderer
    ) {
        return GameObjectBuilder
            .Builder("particle")
            .SetPosition(startPosition)
            .SetScale(startSize)
            .SetRotation(startRotation)
            .SetComponent(new KillParticleScript(lifeTime))
            .SetComponent(new MoveParticleScript(moveDirection))
            .SetComponent(new TextureRenderer(true, renderer.texture, renderer.sortingLayer, renderer.orderInLayer, Color.White(), false, false));
    }
}