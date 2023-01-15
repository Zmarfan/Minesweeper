namespace Worms.engine.game_object.components.particle_system.shape; 

public class Shape {
    private readonly IEmissionShape _emissionShape;
    private readonly float _spread;
    private readonly float _randomizeDirection;

    public Shape(IEmissionShape emissionShape, float spread, float randomizeDirection) {
        _emissionShape = emissionShape;
        _spread = Math.Clamp(spread, 0, 1);
        _randomizeDirection = Math.Clamp(randomizeDirection, 0, 1);
    }
}