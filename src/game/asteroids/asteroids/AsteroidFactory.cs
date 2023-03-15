using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.engine.helper;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.asteroids; 

public static class AsteroidFactory {
    private const float MAX_ANGULAR_VELOCITY = 150;
    
    private static readonly Random RANDOM = new();
    
    public static GameObject Create(Transform parent, AsteroidType type, Vector2 position) {
        AsteroidDetails generalDetails = AsteroidDetails.GetDetails(type);
        AsteroidDetails.AsteroidTypeDetails details = generalDetails.details[RANDOM.Next(generalDetails.details.Count)];
        Vector2 velocity = CalculateInitialVelocity(generalDetails);
        float angularVelocity = RandomUtil.GetRandomValueBetweenTwoValues(RANDOM, -MAX_ANGULAR_VELOCITY, MAX_ANGULAR_VELOCITY);

        GameObject obj = parent.AddChild("asteroid")
            .SetLayer(LayerNames.ASTEROID)
            .SetPosition(position)
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateSingle(details.textureId)).Build())
            .SetComponent(new PolygonCollider(true, details.polygonVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new Asteroid(velocity, angularVelocity, generalDetails))
            .Build()
            .Transform.AddChild("playAreaContainer")
            .SetLayer(LayerNames.PLAY_AREA_OBJECT)
            .SetComponent(new PolygonCollider(true, details.polygonVertices, ColliderState.TRIGGERING_COLLIDER, Vector2.Zero()))
            .Build()
            .Transform.Parent!.gameObject;
        Transform.Instantiate(obj);
        return obj;
    }
    
    private static Vector2 CalculateInitialVelocity(AsteroidDetails details) {
        float speed = RandomUtil.GetRandomValueBetweenTwoValues(RANDOM, details.minVelocity, details.maxVelocity);
        return Vector2.InsideUnitCircle(RANDOM) * speed;
    }
}