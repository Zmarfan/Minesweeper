using Worms.engine.core;
using Worms.engine.core.audio;
using Worms.engine.core.input.listener;
using Worms.engine.scene;
using Worms.game.scenes;

namespace Worms;

internal static class Program {
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
            new List<Scene> { Scene1.GetScene() },
            listeners, 
            new List<string> { "layer1", "layer2", "layer3" },
            new AudioSettings(Volume.Max(), new List<AudioChannel> {
                new("effects", Volume.Max()),
                new("music", Volume.Max()),
            })
        ));
        game.Run();
    }
}