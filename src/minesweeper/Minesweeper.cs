using GameEngine.engine.core;
using GameEngine.engine.core.assets;
using GameEngine.engine.core.cursor;
using GameEngine.engine.core.renderer.textures;
using GameEngine.engine.helper;
using GameEngine.engine.window.menu;
using GameEngine.minesweeper.names;
using GameEngine.minesweeper.scenes;

namespace GameEngine.minesweeper; 

public static class Minesweeper {
    public static Game CreateGame() {
        return new Game(GameSettingsBuilder
            .Builder()
            .SetDebugMode()
            .SetTitle("Minesweeper")
            .SetAssets(DefineAssets())
            .SetWindowIcon(Path("icon.png"))
            .AddScenes(ListUtils.Of(MainScene.GetScene()))
            .SetCursorSettings(new CursorSettings(true, false))
            .SetWindowMenu(WindowMenuBuilder
                .Builder("main_menu")
                .AddDropDown(WindowMenuBuilder
                    .Builder("Game")
                    .AddItem(MenuItemBuilder.Builder(MenuNames.NEW, "New").SetRightText("F2").Build())
                    .AddBreak()
                    .AddItem(MenuItemBuilder.Builder(MenuNames.BEGINNER, "Beginner").Build())
                    .AddItem(MenuItemBuilder.Builder(MenuNames.INTERMEDIATE, "Intermediate").Build())
                    .AddItem(MenuItemBuilder.Builder(MenuNames.EXPERT, "Expert").Build())
                    .AddItem(MenuItemBuilder.Builder(MenuNames.CUSTOM, "Custom...").Build())
                    .AddBreak()
                    .AddItem(MenuItemBuilder.Builder(MenuNames.MARKS, "Marks (?)").Build())
                    .AddBreak()
                    .AddItem(MenuItemBuilder.Builder(MenuNames.BEST_TIMES, "Best Times...").Build())
                    .AddBreak()
                    .AddItem(MenuItemBuilder.Builder(MenuNames.EXIT, "Exit").Build())
                    .Build()
                )
                .AddDropDown(WindowMenuBuilder
                    .Builder("Help")
                    .AddItem(MenuItemBuilder.Builder(MenuNames.ABOUT, "About Minesweeper...").Build())
                    .Build()
                )
                .Build()
            )
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
    
    public static string Path(string path) {
        return $"{Directory.GetCurrentDirectory()}\\src\\assets\\{path}";
    }
}