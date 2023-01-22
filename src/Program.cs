using Worms.engine.core;
using Worms.engine.core.audio;
using Worms.engine.core.input.listener;
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
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.scene;
using Worms.game;

namespace Worms;

internal static class Program {
    private static readonly string ROOT_DIRECTORY = Directory.GetCurrentDirectory();

    private static void Main() {
        List<InputListener> listeners = new() {
            InputListenerBuilder.Builder("horizontal", Button.D)
                .SetNegativeButton(Button.A)
                .SetAltPositiveButton(Button.RIGHT)
                .SetAltNegativeButton(Button.LEFT)
                .SetSnap(false)
                .Build(),
            InputListenerBuilder.Builder("vertical", Button.W)
                .SetNegativeButton(Button.S)
                .SetAltPositiveButton(Button.UP)
                .SetAltNegativeButton(Button.DOWN)
                .SetAxis(InputAxis.Y_AXIS)
                .Build(),
            InputListenerBuilder.Builder("action", Button.RIGHT_MOUSE)
                .SetAltPositiveButton(Button.MIDDLE_MOUSE)
                .Build(),
            InputListenerBuilder.Builder("explosion", Button.SPACE).Build(),
            InputListenerBuilder.Builder("cameraZoom", Button.NUM_1)
                .SetNegativeButton(Button.NUM_2)
                .Build(),
            InputListenerBuilder.Builder("animationTest1", Button.I).Build(),
            InputListenerBuilder.Builder("animationTest2", Button.O).Build(),
            InputListenerBuilder.Builder("animationTest3", Button.P).Build(),
        };
        
        Game game = new(new GameSettings(
            true, 
            "test game",
            1200, 
            800,
            new List<Scene> {
                new("main", () => new MyCamera(), CreateScene1Root )
            },
            listeners, 
            new List<string> { "layer1", "layer2", "layer3" },
            new AudioSettings(Volume.Max(), new List<AudioChannel> {
                new("effects", Volume.Max()),
                new("music", Volume.Max()),
            })
        ));
        game.Run();
    }

    private static string Path(string path) {
        return $"{ROOT_DIRECTORY}\\src\\assets\\test\\{path}";
    }

    private static GameObject CreateScene1Root() {
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
            .Transform.AddChild("background")
            .SetComponent(
                TextureRendererBuilder
                    .Builder(Texture.CreateMultiple(Path("background.png"), 1, 1, 2, 2))
                    .SetSortingLayer("layer3")
                    .SetSortingOrder(0)
                    .Build()
            )
            .Build()
            .Transform.AddSibling("animation")
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
}