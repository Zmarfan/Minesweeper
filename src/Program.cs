using Worms.engine.core;
using Worms.engine.core.assets;
using Worms.engine.core.audio;
using Worms.engine.core.cursor;
using Worms.engine.core.input.listener;
using Worms.engine.core.renderer.textures;
using Worms.engine.helper;
using Worms.game.scenes;

namespace Worms;

internal static class Program {
    private static void Main() {
        Game game = new(GameSettingsBuilder
            .Builder(DefineAssets(), new AudioSettings(Volume.Max(), ListUtils.Of(
                new AudioChannel("effects", Volume.Max()),
                new AudioChannel("music", Volume.Max())
            )))
            .SetDebugMode()
            .SetWindowWidth(1200)
            .SetWindowHeight(800)
            .AddScenes(ListUtils.Of(Scene1.GetScene()))
            .AddInputListeners(ListUtils.Of(
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
                InputListenerBuilder.Builder("cursorToggle", Button.C).Build()
            ))
            .AddLayers(ListUtils.Of("testLayer1", "testLayer2", "testLayer3"))
            .AddSortLayers(ListUtils.Of("layer1", "layer2", "layer3"))
            .SetCursorSettings(new CursorSettings(false, new CustomCursorSettings($"{Directory.GetCurrentDirectory()}\\src\\assets\\test\\cursor.png")))
            .Build()
        );

        game.Run();
    }

    private static Assets DefineAssets() {
        return AssetsBuilder
            .Builder()
            .AddTextures(ListUtils.Of(
                new AssetDeclaration(Path("test\\1.png"), "1"), 
                new AssetDeclaration(Path("test\\explosion\\circle75.png"), "circle75"), 
                new AssetDeclaration(Path("test\\explosion\\elipse75.png"), "elipse75"), 
                new AssetDeclaration(Path("test\\explosion\\expow.png"), "expow"), 
                new AssetDeclaration(Path("test\\explosion\\smklt75.png"), "smklt75"), 
                new AssetDeclaration(Path("test\\explosion\\flame1.png"), "flame1"), 
                new AssetDeclaration(Path("test\\explosion\\smkdrk40.png"), "smkdrk40"), 
                new AssetDeclaration(Path("test\\pixelTest7.png"), "pixelTest7")
            ))
            .AddFonts(ListUtils.Of(
                new AssetDeclaration(Path("test\\Vanilla Caramel.ttf"), "myFont"),
                new AssetDeclaration(Path("test\\Consolas.ttf"), "Consolas"),
                new AssetDeclaration(Path("test\\times-new-roman.ttf"), "times")
            ))
            .AddAudios(ListUtils.Of(
                new AssetDeclaration(Path("test\\explosion\\Explosion1.wav"), "Explosion1")
            ))
            .Build();
    }
    
    private static string Path(string path) {
        return $"{Directory.GetCurrentDirectory()}\\src\\assets\\{path}";
    }
}