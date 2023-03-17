using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.engine.helper;
using Worms.game.asteroids.asteroids;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.controller; 

public static class LifeFactory {
    private static readonly Vector2 LIFE_DISTANCE = new(35, 0);
    
    public static void Create(Transform parent, int lifeCount) {
        for (int i = 0; i < lifeCount; i++) {
            GameObject obj = parent.AddChild("life")
                .SetLocalPosition(LIFE_DISTANCE * i)
                .SetLocalScale(new Vector2(0.8f, 0.8f))
                .SetRotation(Rotation.CounterClockwise())
                .SetComponent(TextureRendererBuilder.Builder(Texture.CreateMultiple(TextureNames.PLAYER, 0, 0, 1, 2)).Build())
                .Build();
            Transform.Instantiate(obj);
        }
    }
}