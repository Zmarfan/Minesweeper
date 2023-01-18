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
                    .Builder(RendererBuilder.Builder(Texture.CreateMultiple(Path("animation_1.png"), 0, 0, 1, 19)).Build())
                    .SetEmission(EmissionBuilder
                        .Builder()
                        .SetRateOverTime(new RangeZero(1))
                        .AddBurst(new EmissionBurst(0, new RangeZero(50, 100), 1, 1, 1))
                        .Build()
                    )
                    .SetParticles(ParticlesBuilder
                        .Builder()
                        .SetWorldSpace()
                        .SetRotationVelocity(new Range(-100, 100))
                        .SetDuration(5f)
                        .SetStartRotation(new RangeZero(0, 25))
                        .SetFlipRotation(0.5f)
                        .SetStartLifeTime(new RangeZero(3, 6))
                        .Build()
                    )
                    .SetParticleAnimation(() => AnimationFactory.CreateTextureAnimation(Path("animation_1.png"), 0.05f, false, 19))
                    .SetShape(new Shape(new SphereEmission(50, 0, Rotation.FromDegrees(359)), new RangeZero(50)))
                    .Build()
            )
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateMultiple(Path("animation_1.png"), 0, 0, 1, 19)).Build())
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
            .SetComponent(new MyTestScript(4.5f))
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