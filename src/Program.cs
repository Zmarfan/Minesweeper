using Worms.engine.core;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components;
using Worms.engine.game_object.components.texture_renderer;
using Worms.game;

namespace Worms;

internal static class Program {
    private static void Main() {
        GameObject root = GameObjectBuilder
            .Builder("rootObject")
            .SetLocalPosition(new Vector2(0, 0))
            .SetLocalRotation(Rotation.Normal())
            .Build()
                .AddChild("child1")
                .SetLocalPosition(new Vector2(0, 0))
                .SetLocalScale(1f)
                .SetComponent(new MyTestScript(2.5f))
                .SetComponent(
                    TextureRendererBuilder
                        .Builder("src\\assets\\test\\5.png")
                        .SetSortingLayer("layer1")
                        .SetSortingOrder(0)
                        .Build()
                )
                .Build()
                    .AddChild("child2Sibling1")
                    .SetLocalPosition(new Vector2(600, 400))
                    .SetLocalScale(2)
                    .SetComponent(new MyTestScript(1.5f))
                    .SetComponent(
                        TextureRendererBuilder
                            .Builder("src\\assets\\test\\4.png")
                            .SetSortingLayer("layer1")
                            .SetSortingOrder(1)
                            .Build()
                    )
                    .Build()
                    .AddSibling("child2Sibling2")
                    .SetLocalPosition(new Vector2(600, -400))
                    .SetLocalRotation(Rotation.UpsideDown())
                    .SetLocalScale(2)
                    .SetComponent(new MyTestScript(4.5f))
                    .SetComponent(
                        TextureRendererBuilder
                            .Builder("src\\assets\\test\\3.png")
                            .SetSortingLayer("layer3")
                            .SetFlipY(true)
                            .Build()
                    )
                    .Build()
                        .AddChild("child2Sibling3")
                        .SetLocalScale(2)
                        .SetLocalPosition(new Vector2(-1200, 0))
                        .SetLocalRotation(Rotation.CounterClockwise())
                        .SetComponent(
                            TextureRendererBuilder
                                .Builder("src\\assets\\test\\2.png")
                                .SetFlipX(true)
                                .Build()
                        )
                        .Build()
                        .AddSibling("child2Sibling4")
                        .SetLocalScale(2)
                        .SetLocalPosition(new Vector2(-1200, 800))
                        .SetLocalRotation(Rotation.Clockwise())
                        .SetComponent(
                            TextureRendererBuilder
                                .Builder("src\\assets\\test\\1.png")
                                .SetColor(new Color(0.25f, 1f, 0.25f))
                                .SetFlipX(true)
                                .SetFlipY(true)
                                .Build()
                        )
                        .Build()
            .GetRoot();
        
        Game game = new(new GameSettings("test game", 1200, 800, new MyCamera(), root, new List<string> { "layer1", "layer2", "layer3" }));
        game.Run();
    }
}