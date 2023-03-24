using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.minesweeper.names;

namespace GameEngine.minesweeper.game.number_display; 

public static class NumberDisplayFactory {
    private const float NUMBER_CENTER_OFFSET = 70;
    
    public static void Create(Transform parent, Vector2 position, string name) {
        GameObject obj = parent.AddChild("numberDisplay")
            .SetComponent(new NumberDisplay(name))
            .SetLocalPosition(position)
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateSingle(TextureNames.NUMBER_DISPLAY)).Build())
            .Build()
                .Transform.AddChild("0")
                .SetLocalPosition(new Vector2(-NUMBER_CENTER_OFFSET, 0))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetNumberTexture(0)).SetName(NumberDisplay.NO_0).Build())
                .Build()
                .Transform.AddSibling("1")
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetNumberTexture(0)).SetName(NumberDisplay.NO_1).Build())
                .Build()
                .Transform.AddSibling("2")
                .SetLocalPosition(new Vector2(NUMBER_CENTER_OFFSET, 0))
                .SetComponent(TextureRendererBuilder.Builder(TextureProvider.GetNumberTexture(0)).SetName(NumberDisplay.NO_2).Build())
                .Build()
            .Transform.Parent!.gameObject;
        Transform.Instantiate(obj);
    }
}