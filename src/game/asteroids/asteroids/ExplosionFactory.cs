using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.particle_system;
using Worms.engine.game_object.components.particle_system.emission;
using Worms.engine.game_object.components.particle_system.particles;
using Worms.engine.game_object.components.particle_system.ranges;
using Worms.engine.game_object.components.particle_system.renderer;
using Worms.engine.game_object.components.particle_system.shape;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.asteroids; 

public static class ExplosionFactory {
    public static void CreateExplosion(Transform parent, Vector2 position, RangeZero particleCount) {
        GameObject obj = parent
            .AddChild("explosion")
            .SetPosition(position)
            .SetComponent(ParticleSystemBuilder
                .Builder(RendererBuilder.Builder(Texture.CreateSingle(TextureNames.FRAGMENT)).Build())
                .SetEmission(EmissionBuilder
                    .Builder()
                    .AddBurst(new EmissionBurst(0, particleCount, 1, 1, 1))
                    .SetRateOverTime(RangeZero.Zero())
                    .Build()
                )
                .SetShape(new Shape(
                    new SphereEmission(1, 1, Rotation.FromDegrees(359)),
                    new VectorRange(new Vector2(30, 30), new Vector2(80, 80))
                ))
                .SetParticles(ParticlesBuilder
                    .Builder()
                    .SetDuration(2f)
                    .SetStartLifeTime(new RangeZero(1f, 2f))
                    .SetLoop(false)
                    .SetStopAction(StopAction.DESTROY)
                    .Build()
                )
                .Build()
            )
            .Build();
        Transform.Instantiate(obj);
    }
}