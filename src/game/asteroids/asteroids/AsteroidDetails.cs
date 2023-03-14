using Worms.engine.data;
using Worms.engine.helper;

namespace Worms.game.asteroids.asteroids; 

public class AsteroidDetails {
    private static readonly AsteroidDetails BIG = new(ListUtils.Of(
        new AsteroidTypeDetails(TextureNames.BIG_ASTEROID_1, new Vector2[] {
            new (-9, -42),
            new (-23, -60),
            new (-56, -27),
            new (-40, 0),
            new (-52, 26),
            new (-23, 58),
            new (3, 42),
            new (28, 58),
            new (57, 25),
            new (37, 10),
            new (57, -11),
            new (30, -58)
        })
    ), 50, 400);

    private static readonly AsteroidDetails MEDIUM = new(ListUtils.Empty<AsteroidTypeDetails>(), 0, 0);

    private static readonly AsteroidDetails SMALL = new(ListUtils.Empty<AsteroidTypeDetails>(), 0, 0);

    public readonly List<AsteroidTypeDetails> details;
    public readonly float minVelocity;
    public readonly float maxVelocity;

    public AsteroidDetails(List<AsteroidTypeDetails> details, float minVelocity, float maxVelocity) {
        this.details = details;
        this.minVelocity = minVelocity;
        this.maxVelocity = maxVelocity;
    }

    public static AsteroidDetails GetDetails(AsteroidType type) {
        return type switch {
            AsteroidType.BIG => BIG,
            AsteroidType.MEDIUM => MEDIUM,
            AsteroidType.SMALL => SMALL,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "No support for this type")
        };
    }
    
    public class AsteroidTypeDetails {
        public readonly string textureId;
        public readonly Vector2[] polygonVertices;

        public AsteroidTypeDetails(string textureId, Vector2[] polygonVertices) {
            this.textureId = textureId;
            this.polygonVertices = polygonVertices;
        }
    }
}