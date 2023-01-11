using Worms.engine.core;
using Worms.engine.core.input.listener;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.texture_renderer;
using Worms.game;

namespace Worms;

internal static class Program {
    private static void Main() {
        GameObject root = GameObjectBuilder.Root()
            .Transform.AddChild("background")
            .SetPosition(new Vector2(0, 0))
            .SetScale(12f)
            .SetComponent(
                TextureRendererBuilder
                    .Builder("src\\assets\\test\\background.png")
                    .SetSortingLayer("layer3")
                    .SetSortingOrder(0)
                    .Build()
            )
            .Build()
            .Transform.AddSibling("child1")
            .SetComponent(new MyTestScript(4.5f))
            .SetPosition(new Vector2(-600, 0))
            .SetScale(new Vector2(2f, 1f))
            .SetComponent(
                TextureRendererBuilder
                    .Builder("src\\assets\\test\\5.png")
                    .SetSortingLayer("layer1")
                    .SetSortingOrder(0)
                    .Build()
            )
            .Build()
                .Transform.AddChild("child2Sibling1")
                .SetPosition(new Vector2(600, 400))
                .SetComponent(
                    TextureRendererBuilder
                        .Builder("src\\assets\\test\\4.png")
                        .SetSortingLayer("layer1")
                        .SetSortingOrder(1)
                        .Build()
                )
                .Build()
                .Transform.AddSibling("child2Sibling2")
                .SetPosition(new Vector2(600, -400))
                .SetScale(0.5f)
                .SetRotation(Rotation.UpsideDown())
                .SetComponent(
                    TextureRendererBuilder
                        .Builder("src\\assets\\test\\3.png")
                        .SetSortingLayer("layer3")
                        .SetFlipY(true)
                        .Build()
                )
                .Build()
                    .Transform.AddChild("child2Sibling3")
                    .SetPosition(new Vector2(-1200, 0))
                    .SetRotation(Rotation.CounterClockwise())
                    .SetComponent(
                        TextureRendererBuilder
                            .Builder("src\\assets\\test\\2.png")
                            .SetFlipX(true)
                            .Build()
                    )
                    .Build()
                    .Transform.AddSibling("child2Sibling4")
                    .SetPosition(new Vector2(-1200, 800))
                    .SetRotation(Rotation.Clockwise())
                    .SetComponent(
                        TextureRendererBuilder
                            .Builder("src\\assets\\test\\1.png")
                            .SetColor(new Color(0.25f, 1f, 0.25f))
                            .SetFlipX(true)
                            .SetFlipY(true)
                            .Build()
                    )
                    .Build()
            .Transform.GetRoot().gameObject;

        List<InputListener> listeners = new() {
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
        };
        
        Game game = new(new GameSettings("test game", 1200, 800, new MyCamera(), root, listeners, new List<string> { "layer1", "layer2", "layer3" }));
        game.Run();
    }
}