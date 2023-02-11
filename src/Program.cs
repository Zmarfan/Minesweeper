using Worms.engine.core;
using Worms.engine.core.audio;
using Worms.engine.core.cursor;
using Worms.engine.core.input.listener;
using Worms.engine.core.renderer.font;
using Worms.game.scenes;

namespace Worms;

internal static class Program {
    private static void Main() {
        Game game = new(GameSettingsBuilder
            .Builder(new AudioSettings(Volume.Max(), new List<AudioChannel> {
                new("effects", Volume.Max()),
                new("music", Volume.Max()),
            }))
            .SetDebugMode()
            .SetWindowWidth(1200)
            .SetWindowHeight(800)
            .AddScenes(Scene1.GetScene())
            .AddInputListeners(
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
                InputListenerBuilder.Builder("alterTexture", Button.NUM_7).Build(),
                InputListenerBuilder.Builder("explosion", Button.SPACE).Build(),
                InputListenerBuilder.Builder("cameraZoom", Button.NUM_1)
                    .SetNegativeButton(Button.NUM_2)
                    .Build(),
                InputListenerBuilder.Builder("animationTest1", Button.I).Build(),
                InputListenerBuilder.Builder("animationTest2", Button.O).Build(),
                InputListenerBuilder.Builder("animationTest3", Button.P).Build(),
                InputListenerBuilder.Builder("cursorToggle", Button.C).Build()
            )
            .AddSortLayers("layer1", "layer2", "layer3")
            .SetCursorSettings(new CursorSettings(false, new CustomCursorSettings($"{Directory.GetCurrentDirectory()}\\src\\assets\\test\\cursor.png")))
            .AddFont(new FontDefinition($"{Directory.GetCurrentDirectory()}\\src\\assets\\test\\Vanilla Caramel.ttf", "myFont"))
            .AddFont(new FontDefinition($"{Directory.GetCurrentDirectory()}\\src\\assets\\test\\Consolas.ttf", "Consolas"))
            .AddFont(new FontDefinition($"{Directory.GetCurrentDirectory()}\\src\\assets\\test\\times-new-roman.ttf", "times"))
            .Build()
        );

        game.Run();
    }
}