using Worms.engine;
using Worms.engine.core;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.texture_renderer;

namespace Worms;

internal static class Program {
    private static void Main() {
        GameObject root = GameObjectBuilder
            .Builder("rootObject")
            .SetLocalPosition(new Vector2(2, 2))
            .SetLocalRotation(Rotation.Clockwise())
            .Build()
                .AddChild("child1")
                .SetWorldScale(0.5f)
                .SetWorldPosition(new Vector2(1, 3))
                .SetWorldRotation(Rotation.Clockwise())
                .Build()
                    .AddChild("child2Sibling1")
                    .SetLocalPosition(new Vector2(3, -2))
                    .Build()
                    .AddSibling("child2Sibling2")
                    .SetLocalPosition(new Vector2(4, -2))
                    .SetWorldRotation(Rotation.UpsideDown())
                    .Build()
                    .AddSibling("child2Sibling3")
                    .SetLocalScale(0.25f)
                    .SetLocalPosition(new Vector2(5, -2))
                    .SetWorldRotation(Rotation.UpsideDown())
                    .Build()
                        .AddChild("child2Sibling4")
                        .SetWorldScale(0.5f)
                        .SetLocalPosition(new Vector2(6, -2))
                        .SetWorldRotation(Rotation.UpsideDown())
                        .SetComponent(TextureRendererBuilder.Builder("src/image.png").Build())
                        .Build()
            .GetRoot();
        
        Console.WriteLine(root);
        Game game = new(new GameSettings("test game", 600, 400, root));
        game.Run();
    }
}