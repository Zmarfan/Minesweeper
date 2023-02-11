using Worms.engine.game_object.components.animation.composition;
using Worms.engine.game_object.components.rendering.texture_renderer;

namespace Worms.engine.game_object.components.animation.animation; 

public static class AnimationFactory {
    public static Animation CreateTextureAnimation(string textureSrc, float stepLengthInSeconds, bool loop, int frameCount, bool reverse = false) {
        List<State> states = new();
        for (int i = 0; i < frameCount; i++) {
            int row = i;
            states.Add(new State(
                component => {
                    TextureRenderer tr = (TextureRenderer)component;
                    tr.texture = Texture.CreateMultiple(textureSrc, 0, row, 1, frameCount);
                },
                1
            ));
        }

        if (reverse) {
            states.Reverse();
        }
        Composition composition = new(
            gameObject => gameObject.GetComponent<TextureRenderer>(),
            states
        );
        return new Animation(stepLengthInSeconds, loop, new List<Composition> { composition });
    }
}