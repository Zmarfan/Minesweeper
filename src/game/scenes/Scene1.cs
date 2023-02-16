using Worms.engine.core.audio;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.animation.animation;
using Worms.engine.game_object.components.animation.controller;
using Worms.engine.game_object.components.audio_source;
using Worms.engine.game_object.components.particle_system;
using Worms.engine.game_object.components.particle_system.emission;
using Worms.engine.game_object.components.particle_system.particles;
using Worms.engine.game_object.components.particle_system.ranges;
using Worms.engine.game_object.components.particle_system.renderer;
using Worms.engine.game_object.components.particle_system.shape;
using Worms.engine.game_object.components.physics;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.text_renderer;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.engine.game_object.components.screen_pivot;
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
                    .Builder(Texture.CreateMultiple("circle75", 0, 0, 1, 4))
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
                    AnimationFactory.CreateTextureAnimation("circle75", 0.05f, false, 4))
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
                .Builder(Texture.CreateMultiple("elipse75", 0, 0, 1, 10))
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
                AnimationFactory.CreateTextureAnimation("elipse75", 0.05f, false, 10))
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
                .Builder(Texture.CreateMultiple("expow", 0, 0, 1, 12))
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
                AnimationFactory.CreateTextureAnimation("expow", 0.05f, false, 12))
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
                .Builder(Texture.CreateMultiple("smklt75", 0, 0, 1, 28))
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
                AnimationFactory.CreateTextureAnimation("smklt75", 0.1f, false, 28))
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
                .Builder(Texture.CreateMultiple("flame1", 0, 0, 1, 32))
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
                AnimationFactory.CreateTextureAnimation("flame1", 0.1f, false, 32))
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
                    .Builder(Texture.CreateMultiple("smkdrk40", 0, 0, 1, 28))
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
                    AnimationFactory.CreateTextureAnimation("smkdrk40", 0.1f, false, 28))
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
            .SetComponent(TextureColliderBuilder
                .Builder(Texture.CreateSingle("pixelTest7"))
                .SetState(ColliderState.TRIGGERING_COLLIDER)
                .Build()
            )
            .Build()
            .Transform.AddSibling("circleCollider1")
            .SetPosition(new Vector2(200, 100))
            .SetComponent(new BoxCollider(true, ColliderState.TRIGGERING_COLLIDER,  new Vector2(200, 200), Vector2.Zero()))
            .SetRotation(Rotation.FromDegrees(0))
            .SetScale(new Vector2(1, 2))
            .Build()
            .Transform.AddSibling("text")
            .SetPosition(new Vector2(-200, 100))
            .SetComponent(new CircleCollider(true, ColliderState.TRIGGERING_COLLIDER, 150f, Vector2.Zero()))
            .Build()
            .Transform.AddSibling("circleCollider4")
            .SetPosition(new Vector2(100, 100))
            .SetComponent(new CircleCollider(true, ColliderState.TRIGGERING_COLLIDER, 120, Vector2.Zero()))
            .Build()
            .Transform.AddSibling("animation")
            .SetComponent(new TriggerScriptTest())
            .SetComponent(TextureColliderBuilder
                .Builder(Texture.CreateSingle("1"))
                .Build()
            )
            .SetComponent(AudioSourceBuilder
                .Builder(Path("explosion\\Explosion1.wav"), "effects")
                .SetVolume(Volume.FromPercentage(10))
                .SetPlayOnAwake(false)
                .Build()
            )
            .SetComponent(TextRendererBuilder
                .Builder()
                .SetText("VaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVaVa Lorem aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa ipsum dolor sit amet, consectetur\n\n adipiscing aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa elit. Ut porttitor purus in nisi maximus suscipit. Proin nulla orci, scelerisque in tempus eget, vehicula non enim. In ut augue iaculis, elementum ligula nec, tincidunt magna. Suspendisse vel ex vitae orci sollicitudin pulvinar. Sed tempus lorem sollicitudin accumsan luctus. Maecenas vitae tincidunt est. Fusce pulvinar, tellus ut hendrerit luctus, lacus lorem varius elit, nec accumsan mauris dolor vel lorem. Duis tincidunt porta sollicitudin. Ut imperdiet luctus placerat. Pellentesque vel nibh vel felis semper pulvinar. Nullam malesuada malesuada justo, in vehicula tellus commodo in. Nam rhoncus tristique faucibus. meramera")
                .SetFont("times")
                .SetAlignment(TextAlignment.RIGHT)
                .SetSize(35)
                .SetWidth(800)
                .SetSortingOrder(-100)
                .Build()
            )
            .SetComponent(new RigidBody())
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