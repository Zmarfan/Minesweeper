using Worms.engine.data;
using Worms.engine.game_object.components.particle_system.ranges;

namespace Worms.engine.game_object.components.particle_system.shape; 

public class Shape {
    private readonly IEmissionShape _emissionShape;
    private readonly VectorRange _startSpeed;
    private readonly float _randomizeDirection;

    public Shape(IEmissionShape emissionShape, VectorRange startSpeed, float randomizeDirection = 0) {
        _emissionShape = emissionShape;
        _startSpeed = startSpeed;
        _randomizeDirection = Math.Clamp(randomizeDirection, 0, 1);
    }

    public Tuple<Vector2, Vector2> GetSpawnPositionAndDirection(Random random) {
        Tuple<Vector2, Vector2> positionAndDirection = _emissionShape.GetSpawnPositionAndDirection(random);
        return new Tuple<Vector2, Vector2>(
            positionAndDirection.Item1,
            CreateDirection(positionAndDirection.Item2, random)
        );
    }

    private Vector2 CreateDirection(Vector2 baseDirection, Random random) {
        return Vector2.Lerp(
            baseDirection, 
            GenerateRandomDirection(random), 
            random.NextDouble() < _randomizeDirection ? (float)random.NextDouble() : 0 
        ) * _startSpeed.GetRandom(random);
    }

    private static Vector2 GenerateRandomDirection(Random random) {
        float degree = (float)(random.NextDouble() * 360);
        return new Vector2((float)Math.Cos(degree), (float)Math.Sin(degree));
    }
}