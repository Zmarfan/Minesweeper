using Worms.engine.core.audio;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.animation.animation;
using Worms.engine.game_object.components.animation.controller;
using Worms.engine.game_object.components.audio_source;
using Worms.engine.game_object.components.colliders;
using Worms.engine.game_object.components.particle_system;
using Worms.engine.game_object.components.particle_system.emission;
using Worms.engine.game_object.components.particle_system.particles;
using Worms.engine.game_object.components.particle_system.ranges;
using Worms.engine.game_object.components.particle_system.renderer;
using Worms.engine.game_object.components.particle_system.shape;
using Worms.engine.game_object.components.screen_pivot;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.scene;

namespace Worms.game.scenes; 

public static class Scene1 {
    public static Scene GetScene() {
        return new Scene("main", () => new MyCamera(), CreateWorldRoot, CreateScreenRoot);
    }
    
    private static GameObject CreateWorldRoot() {
        Func<GameObjectBuilder> explosion = () => GameObjectBuilder.Builder("explosion")
            .SetComponent(ParticleSystemBuilder
                .Builder(RendererBuilder
                    .Builder(Texture.CreateMultiple(Path("explosion/circle75.png"), 0, 0, 1, 4))
                    .SetSortingOrder(1)
                    .Build()
                )
                .SetEmission(EmissionBuilder
                    .Builder()
                    .SetRateOverTime(RangeZero.Zero())
                    .AddBurst(new EmissionBurst(0, new RangeZero(1), 1, 1, 1))
                    .Build()
                )
                .SetParticleAnimation(() =>
                    AnimationFactory.CreateTextureAnimation(Path("explosion/circle75.png"), 0.05f, false, 4))
                .SetShape(new Shape(new LineEmission(0.01f), new VectorRange(Vector2.Zero())))
                .SetParticles(ParticlesBuilder
                    .Builder()
                    .SetWorldSpace()
                    .SetLoop(false)
                    .SetStopAction(StopAction.DESTROY)
                    .SetDuration(6f)
                    .SetStartLifeTime(new RangeZero(0.4f))
                    .Build()
                )
                .Build()
        )
        .SetComponent(ParticleSystemBuilder
            .Builder(RendererBuilder
                .Builder(Texture.CreateMultiple(Path("explosion/elipse75.png"), 0, 0, 1, 10))
                .SetSortingOrder(0)
                .Build()
            )
            .SetEmission(EmissionBuilder
                .Builder()
                .SetRateOverTime(RangeZero.Zero())
                .AddBurst(new EmissionBurst(0, new RangeZero(1), 1, 1, 1))
                .Build()
            )
            .SetParticleAnimation(() =>
                AnimationFactory.CreateTextureAnimation(Path("explosion/elipse75.png"), 0.05f, false, 10))
            .SetShape(new Shape(new LineEmission(0.01f), new VectorRange(Vector2.Zero())))
            .SetParticles(ParticlesBuilder
                .Builder()
                .SetLoop(false)
                .SetWorldSpace()
                .SetDuration(6f)
                .SetMaxParticles(1)
                .SetStartLifeTime(new RangeZero(0.5f))
                .Build()
            )
            .Build()
        )
        .SetComponent(ParticleSystemBuilder
            .Builder(RendererBuilder
                .Builder(Texture.CreateMultiple(Path("explosion/expow.png"), 0, 0, 1, 12))
                .SetSortingOrder(2)
                .Build()
            )
            .SetEmission(EmissionBuilder
                .Builder()
                .SetRateOverTime(RangeZero.Zero())
                .AddBurst(new EmissionBurst(0, new RangeZero(1), 1, 1, 1))
                .Build()
            )
            .SetParticleAnimation(() =>
                AnimationFactory.CreateTextureAnimation(Path("explosion/expow.png"), 0.05f, false, 12))
            .SetShape(new Shape(new LineEmission(0.01f), new VectorRange(Vector2.Zero())))
            .SetParticles(ParticlesBuilder
                .Builder()
                .SetWorldSpace()
                .SetLoop(false)
                .SetDuration(6f)
                .SetStartSize(new RangeZero(1.25f))
                .SetMaxParticles(1)
                .SetStartDelay(new RangeZero(0.2f))
                .SetStartLifeTime(new RangeZero(0.65f))
                .Build()
            )
            .Build()
        )
        .SetComponent(ParticleSystemBuilder
            .Builder(RendererBuilder
                .Builder(Texture.CreateMultiple(Path("explosion/smklt75.png"), 0, 0, 1, 28))
                .SetSortingOrder(2)
                .Build()
            )
            .SetEmission(EmissionBuilder
                .Builder()
                .SetRateOverTime(RangeZero.Zero())
                .AddBurst(new EmissionBurst(0, new RangeZero(4, 8), 1, 1, 1))
                .AddBurst(new EmissionBurst(0.05f, new RangeZero(4, 8), 1, 1, 1))
                .Build()
            )
            .SetParticleAnimation(() =>
                AnimationFactory.CreateTextureAnimation(Path("explosion/smklt75.png"), 0.1f, false, 28))
            .SetShape(new Shape(new SphereEmission(10f, 1f, Rotation.FromDegrees(180)),
                new VectorRange(new Vector2(-45, 30), new Vector2(45, 50)), 0.4f))
            .SetParticles(ParticlesBuilder
                .Builder()
                .SetWorldSpace()
                .SetLoop(false)
                .SetDuration(6f)
                .SetForceOverLifeTime(new VectorRange(Vector2.Up() * 50))
                .SetStartLifeTime(new RangeZero(2.5f))
                .Build()
            )
            .Build()
        )
        .SetComponent(ParticleSystemBuilder
            .Builder(RendererBuilder
                .Builder(Texture.CreateMultiple(Path("explosion/flame1.png"), 0, 0, 1, 32))
                .SetSortingOrder(2)
                .Build()
            )
            .SetEmission(EmissionBuilder
                .Builder()
                .SetRateOverTime(RangeZero.Zero())
                .AddBurst(new EmissionBurst(0, new RangeZero(4), 1, 1, 1))
                .Build()
            )
            .SetParticleAnimation(() =>
                AnimationFactory.CreateTextureAnimation(Path("explosion/flame1.png"), 0.1f, false, 32))
            .SetShape(new Shape(new SphereEmission(10f, 1f, Rotation.FromDegrees(180)),
                new VectorRange(new Vector2(-200, 600), new Vector2(200, 600))))
            .SetParticles(ParticlesBuilder
                .Builder()
                .SetLoop(false)
                .SetWorldSpace()
                .SetDuration(6f)
                .SetForceOverLifeTime(new VectorRange(Vector2.Down() * 400))
                .SetStartLifeTime(new RangeZero(6f))
                .Build()
            )
            .SetSubSystem(ParticleSystemBuilder
                .Builder(RendererBuilder
                    .Builder(Texture.CreateMultiple(Path("explosion/smkdrk40.png"), 0, 0, 1, 28))
                    .SetSortingOrder(2)
                    .Build()
                )
                .SetEmission(EmissionBuilder
                    .Builder()
                    .SetRateOverTime(new RangeZero(0.5f, 1.5f))
                    .AddBurst(new EmissionBurst(0, new RangeZero(4, 8), 1, 1, 1))
                    .Build()
                )
                .SetParticleAnimation(() =>
                    AnimationFactory.CreateTextureAnimation(Path("explosion/smkdrk40.png"), 0.1f, false, 28))
                .SetShape(new Shape(new SphereEmission(1f, 1f, Rotation.FromDegrees(359)),
                    new VectorRange(Vector2.Zero())))
                .SetParticles(ParticlesBuilder
                    .Builder()
                    .SetLoop(false)
                    .SetWorldSpace()
                    .SetDuration(4f)
                    .SetForceOverLifeTime(new VectorRange(Vector2.Up() * 25))
                    .SetStartLifeTime(new RangeZero(6f))
                    .Build()
                )
            )
            .Build()
        );
        
        return GameObjectBuilder.Root()
            .Transform.AddChild("pixelTest1")
            .SetComponent(new TriggerScriptTest())
            .SetComponent(new TextureCollider(true, true, true, TextureRendererBuilder
                .Builder(Texture.CreateSingle(Path("pixelTest7.png")))
                .SetSortingOrder(4)
            ))
            .Build()
            .Transform.AddSibling("animation")
            .SetScale(2)  
            .SetComponent(new TriggerScriptTest())
            .SetComponent(new CircleCollider(true, true, 30, Vector2.Zero()))
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateMultiple(Path("animation_1.png"), 0, 0, 1, 19)).Build())
            .SetComponent(AudioSourceBuilder
                .Builder(Path("explosion/Explosion1.wav"), "effects")
                .SetVolume(Volume.FromPercentage(10))
                .SetPlayOnAwake(false)
                .Build()
            )
            .SetComponent(new GizmoScript())
            .SetComponent(
                AnimationControllerBuilder
                    .Builder()
                    .AddAnimation("trigger1", AnimationFactory.CreateTextureAnimation(Path("animation_1.png"), 0.05f, false, 19))
                    .AddAnimation("trigger2", AnimationFactory.CreateTextureAnimation(Path("animation_2.png"), 0.05f, false, 15))
                    .AddAnimation("trigger3", AnimationFactory.CreateTextureAnimation(Path("animation_2.png"), 0.05f, false, 15, true))
                    .SetStartAnimation(0)
                    .Build()
            )
            .SetComponent(new AnimationTestScript())
            .SetPosition(new Vector2(-100, -100))
            .SetComponent(new MyTestScript(explosion))
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
    
    private static string Path(string path) {
        return $"{Directory.GetCurrentDirectory()}\\src\\assets\\test\\{path}";
    }
}