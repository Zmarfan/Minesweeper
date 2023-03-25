using GameEngine.engine.core;
using GameEngine.engine.core.assets;
using GameEngine.engine.core.cursor;
using GameEngine.engine.core.renderer.textures;
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
            .SetWindowIcon(Path("icon.png"))
            .AddScenes(ListUtils.Of(Scene1.GetScene()))
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
                new AssetDeclaration(Path("numbers.png"), TextureNames.NUMBERS),
                new AssetDeclaration(Path("smileys.png"), TextureNames.SMILEYS)
            ))
            .AddAudios(ListUtils.Empty<AssetDeclaration>())
            .AddFonts(ListUtils.Empty<AssetDeclaration>())
            .Build();
    }
    
    private static string Path(string path) {
        return $"{Directory.GetCurrentDirectory()}\\src\\assets\\{path}";
    }
}