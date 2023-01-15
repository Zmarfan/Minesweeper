namespace Worms.engine.game_object.components.particle_system.shape; 

public class Shape {
    private readonly IEmissionShape _emissionShape;
    private readonly float _randomizeDirection;

    public Shape(IEmissionShape emissionShape, float randomizeDirection) {
        _emissionShape = emissionShape;
        _randomizeDirection = Math.Clamp(randomizeDirection, 0, 1);
    }
}