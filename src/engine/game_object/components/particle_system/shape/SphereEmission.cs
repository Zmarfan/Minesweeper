using Worms.engine.data;

namespace Worms.engine.game_object.components.particle_system.shape; 

public class SphereEmission : IEmissionShape {
    private readonly float _radius;
    private readonly RangeZero _radiusThickness;
    private readonly Rotation _arc;

    public SphereEmission(float radius, float radiusThickness, Rotation arc) {
        _radius = radius;
        _radiusThickness = new RangeZero(1 - radiusThickness, 1);
        _arc = arc;
    }

    public Tuple<Vector2, Vector2> GetSpawnPositionAndDirection(Random random) {
        Vector2 point = GetPointAlongCircle(random);
        return new Tuple<Vector2, Vector2>(point * _radius * _radiusThickness.GetRandom(random), point);
    }

    private Vector2 GetPointAlongCircle(Random random) {
        Rotation rotation = Rotation.FromDegrees((float)(random.NextDouble() * _arc.Degree));
        return new Vector2((float)Math.Cos(rotation.Radians), (float)Math.Sin(rotation.Radians));
    }
}