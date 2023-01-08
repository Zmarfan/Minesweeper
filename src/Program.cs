using Worms.engine.camera;
using Worms.engine.core;
using Worms.engine.data;
using Worms.engine.game_object;
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
                .SetLocalRotation(new Rotation(45))
                .SetComponent(new RotateSizeScript(true, 0.01f))
                .SetComponent(TextureRendererBuilder.Builder("src\\assets\\test\\5.png").Build())
                .Build()
                    .AddChild("child2Sibling1")
                    .SetLocalPosition(new Vector2(600, 400))
                    .SetComponent(TextureRendererBuilder.Builder("src\\assets\\test\\4.png").Build())
                    .Build()
                    .AddSibling("child2Sibling2")
                    .SetLocalPosition(new Vector2(600, -400))
                    .SetLocalRotation(Rotation.UpsideDown())
                    .SetLocalScale(1)
                    .SetComponent(
                        TextureRendererBuilder
                            .Builder("src\\assets\\test\\3.png")
                            .SetFlipY(true)
                            .Build()
                    )
                    .SetComponent(new RotateSizeScript(true, 0.05f))
                    .Build()
                        .AddChild("child2Sibling3")
                        .SetLocalScale(1)
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
                        .SetLocalScale(0.5f)
                        .SetComponent(new RotateSizeScript(true, 0.1f))
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
        
        Game game = new(new GameSettings("test game", 1200, 800, new MyCamera(), root));
        game.Run();
    }
}