using Worms.engine.core;
using Worms.engine.core.audio;
using Worms.engine.core.cursor;
using Worms.engine.core.input.listener;
using Worms.engine.core.renderer.font;
using Worms.engine.core.renderer.textures;
using Worms.engine.helper;
using Worms.game.scenes;

namespace Worms;

internal static class Program {
    private static void Main() {
        Game game = new(GameSettingsBuilder
            .Builder(new AudioSettings(Volume.Max(), ListUtils.Of(
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
            .AddSortLayers(ListUtils.Of("layer1", "layer2", "layer3"))
            .SetCursorSettings(new CursorSettings(false, new CustomCursorSettings($"{Directory.GetCurrentDirectory()}\\src\\assets\\test\\cursor.png")))
            .AddFonts(ListUtils.Of(
                new FontDefinition(Path("test\\Vanilla Caramel.ttf"), "myFont"),
                new FontDefinition(Path("test\\Consolas.ttf"), "Consolas"),
                new FontDefinition(Path("test\\times-new-roman.ttf"), "times")
            ))
            .AddTextures(ListUtils.Of(
                new TextureDeclaration(Path("test\\1.png"), "1"), 
                new TextureDeclaration(Path("test\\explosion\\circle75.png"), "circle75"), 
                new TextureDeclaration(Path("test\\explosion\\elipse75.png"), "elipse75"), 
                new TextureDeclaration(Path("test\\explosion\\expow.png"), "expow"), 
                new TextureDeclaration(Path("test\\explosion\\smklt75.png"), "smklt75"), 
                new TextureDeclaration(Path("test\\explosion\\flame1.png"), "flame1"), 
                new TextureDeclaration(Path("test\\explosion\\smkdrk40.png"), "smkdrk40"), 
                new TextureDeclaration(Path("test\\pixelTest7.png"), "pixelTest7")
            ))
            .Build()
        );

        game.Run();
    }
    
    private static string Path(string path) {
        return $"{Directory.GetCurrentDirectory()}\\src\\assets\\{path}";
    }
}