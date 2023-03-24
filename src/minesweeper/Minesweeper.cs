using GameEngine.engine.core;
using GameEngine.engine.core.assets;
using GameEngine.engine.core.audio;
using GameEngine.engine.core.cursor;
using GameEngine.engine.core.input.listener;
using GameEngine.engine.core.renderer.textures;
using GameEngine.engine.core.update.physics.layers;
using GameEngine.engine.core.update.physics.settings;
using GameEngine.engine.helper;
using GameEngine.minesweeper.names;
using GameEngine.minesweeper.scenes;

namespace GameEngine.minesweeper; 

public static class Minesweeper {
    public static Game CreateGame() {
        return new Game(GameSettingsBuilder
            .Builder()
            .SetBuildMode()
            .SetTitle("Minesweeper")
            .SetAssets(DefineAssets())
            .SetAudioSettings(new AudioSettings(Volume.Max(), ListUtils.Empty<AudioChannel>()))
            .SetWindowWidth(275)   
            .SetWindowHeight(350)
            .SetWindowIcon(Path("icon.png"))
            .AddScenes(ListUtils.Of(Scene1.GetScene()))
            .AddInputListeners(ListUtils.Of(
                InputListenerBuilder.Builder(InputNames.LEFT_CLICK, Button.LEFT_MOUSE).Build(),
                InputListenerBuilder.Builder(InputNames.RIGHT_CLICK, Button.RIGHT_MOUSE).Build()
            ))
            .SetPhysics(PhysicsSettingsBuilder
                .Builder(ListUtils.Of(LayerMask.DEFAULT), ListUtils.Of(LayerMask.IGNORE_RAYCAST))
                .Build()
            )
            .SetCursorSettings(new CursorSettings(true, false))
            .Build()
        );
    }
    
    private static Assets DefineAssets() {
        return AssetsBuilder
            .Builder()
            .AddTextures(ListUtils.Of(
                new AssetDeclaration(Path("tiles.png"), TextureNames.TILES),
                new AssetDeclaration(Path("border.png"), TextureNames.BORDER),
                new AssetDeclaration(Path("nums_background.png"), TextureNames.NUMBER_DISPLAY),
                new AssetDeclaration(Path("numbers.png"), TextureNames.NUMBERS)
            ))
            .AddAudios(ListUtils.Empty<AssetDeclaration>())
            .AddFonts(ListUtils.Empty<AssetDeclaration>())
            .Build();
    }
    
    private static string Path(string path) {
        return $"{Directory.GetCurrentDirectory()}\\src\\assets\\{path}";
    }
}