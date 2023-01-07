using Worms.engine;
using Worms.engine.camera;
using Worms.engine.core;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.texture_renderer;

namespace Worms;

internal static class Program {
    private static void Main() {
        GameObject root = GameObjectBuilder
            .Builder("rootObject")
            .SetLocalPosition(new Vector2(0, 0))
            .SetLocalRotation(Rotation.Clockwise())
            .Build()
                .AddChild("child1")
                .SetWorldScale(0.5f)
                .SetWorldPosition(new Vector2(0, 0))
                .SetWorldRotation(Rotation.Clockwise())
                .SetComponent(TextureRendererBuilder.Builder("src\\assets\\test\\5.png").Build())
                .Build()
                    .AddChild("child2Sibling1")
                    .SetLocalPosition(new Vector2(400, 200))
                    .SetComponent(TextureRendererBuilder.Builder("src\\assets\\test\\4.png").Build())
                    .Build()
                    .AddSibling("child2Sibling2")
                    .SetLocalPosition(new Vector2(-400, -200))
                    .SetWorldRotation(Rotation.UpsideDown())
                    .SetComponent(TextureRendererBuilder.Builder("src\\assets\\test\\3.png").Build())
                    .Build()
                    .AddSibling("child2Sibling3")
                    .SetLocalScale(0.25f)
                    .SetLocalPosition(new Vector2(600, -200))
                    .SetWorldRotation(Rotation.UpsideDown())
                    .SetComponent(TextureRendererBuilder.Builder("src\\assets\\test\\2.png").Build())
                    .Build()
                        .AddChild("child2Sibling4")
                        .SetWorldScale(0.5f)
                        .SetWorldPosition(new Vector2(600, 400))
                        .SetWorldRotation(Rotation.UpsideDown())
                        .SetComponent(TextureRendererBuilder.Builder("src\\assets\\test\\1.png").Build())
                        .Build()
            .GetRoot();
        
        Game game = new(new GameSettings("test game", 1200, 800, new Camera(), root));
        game.Run();
    }
}