using Worms.engine.core;
using Worms.engine.core.audio;
using Worms.engine.core.input.listener;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.animation.animation;
using Worms.engine.game_object.components.animation.controller;
using Worms.engine.game_object.components.particle_system;
using Worms.engine.game_object.components.particle_system.emission;
using Worms.engine.game_object.components.particle_system.particles;
using Worms.engine.game_object.components.particle_system.ranges;
using Worms.engine.game_object.components.particle_system.renderer;
using Worms.engine.game_object.components.particle_system.shape;
using Worms.engine.game_object.components.texture_renderer;
using Worms.game;
using Range = Worms.engine.game_object.components.particle_system.ranges.Range;

namespace Worms;

internal static class Program {
    private static readonly string ROOT_DIRECTORY = Directory.GetCurrentDirectory();
    
    private static void Main() {
        GameObject root = GameObjectBuilder.Root()
            .Transform.AddChild("background")
            .SetPosition(new Vector2(0, 0))
            .SetScale(12f)
            .SetComponent(
                TextureRendererBuilder
                    .Builder(Texture.CreateMultiple(Path("background.png"), 1, 1, 2, 2))
                    .SetSortingLayer("layer3")
                    .SetSortingOrder(0)
                    .Build()
            )
            .Build()
            .Transform.AddSibling("animation")
            .SetComponent(ParticleSystemBuilder
                    .Builder(RendererBuilder.Builder(Texture.CreateSingle(Path("5.png"))).Build())
                    .SetEmission(EmissionBuilder
                        .Builder()
                        .SetRateOverTime(new RangeZero(3))
                        .AddBurst(new EmissionBurst(2, new RangeZero(100), 1, 1, 1))
                        .Build()
                    )
                    .SetParticles(ParticlesBuilder
                        .Builder()
                        .SetRotationVelocity(new Range(-100, 100))
                        .SetForceOverLifeTime(new VectorRange(new Vector2(0, -500f)))
                        .SetDuration(5f)
                        .SetStartSize(new RangeZero(0.01f, 0.05f))
                        .SetStartRotation(new RangeZero(0, 25))
                        .SetFlipRotation(0.5f)
                        .SetStartLifeTime(new RangeZero(3, 6))
                        .Build()
                    )
                    .SetShape(new Shape(new SphereEmission(45, 1, Rotation.FromDegrees(359)), new RangeZero(10, 40)))
                    .Build())
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateMultiple(Path("animation_1.png"), 0, 0, 1, 19)).Build())
            .SetComponent(
                AnimationControllerBuilder
                    .Builder()
                    .AddAnimation("trigger1", AnimationFactory.CreateTextureAnimation(Path("animation_1.png"), 0.05f, false, 19))
                    .AddAnimation("trigger2", AnimationFactory.CreateTextureAnimation(Path("animation_2.png"), 0.05f, false, 15))
                    .AddAnimation("trigger3", AnimationFactory.CreateTextureAnimation(Path("animation_2.png"), 0.05f, false, 15, true))
                    .Build()
            )
            .SetComponent(new AnimationTestScript())
            .SetScale(new Vector2(12, 12))
            .SetPosition(new Vector2(-500, -500))
            .SetComponent(new MyTestScript(4.5f))
            .Build()
            .Transform.AddSibling("child1")
            .SetComponent(new GizmoScript())
            .SetPosition(new Vector2(-600, 0))
            .SetScale(new Vector2(2, 2))
            .SetComponent(
                TextureRendererBuilder
                    .Builder(Texture.CreateSingle(Path("5.png")))
                    .SetSortingLayer("layer1")
                    .SetSortingOrder(0)
                    .Build()
            )
            .Build()
                .Transform.AddChild("child2Sibling1")
                .SetPosition(new Vector2(600, 400))
                .SetComponent(new GizmoScript())
                .SetComponent(
                    TextureRendererBuilder
                        .Builder(Texture.CreateSingle(Path("4.png")))
                        .SetSortingLayer("layer1")
                        .SetSortingOrder(1)
                        .Build()
                )
                .Build()
                .Transform.AddSibling("child2Sibling2")
                .SetPosition(new Vector2(600, -400))
                .SetScale(0.5f)
                .SetRotation(Rotation.UpsideDown())
                .SetComponent(new GizmoScript())
            .SetComponent(
                    TextureRendererBuilder
                        .Builder(Texture.CreateSingle(Path("3.png")))
                        .SetSortingLayer("layer3")
                        .SetFlipY(true)
                        .Build()
                )
                .Build()
                    .Transform.AddChild("child2Sibling3")
                    .SetPosition(new Vector2(-1200, 0))
                    .SetRotation(Rotation.CounterClockwise())
                    .SetComponent(new GizmoScript())
                    .SetComponent(
                        TextureRendererBuilder
                            .Builder(Texture.CreateSingle(Path("2.png")))
                            .SetFlipX(true)
                            .Build()
                    )
                    .Build()
                    .Transform.AddSibling("child2Sibling4")
                    .SetPosition(new Vector2(-1200, 800))
                    .SetRotation(Rotation.Clockwise())
                    .SetComponent(new GizmoScript())
                    .SetComponent(
                        TextureRendererBuilder
                            .Builder(Texture.CreateSingle(Path("1.png")))
                            .SetColor(new Color(0.25f, 1f, 0.25f))
                            .SetFlipX(true)
                            .SetFlipY(true)
                            .Build()
                    )
                    .Build()
            .Transform.GetRoot().gameObject;

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
            new MyCamera(),
            root,
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
}