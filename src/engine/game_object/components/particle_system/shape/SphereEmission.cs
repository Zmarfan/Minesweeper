using Worms.engine.data;

namespace Worms.engine.game_object.components.particle_system.shape; 

public class SphereEmission : IEmissionShape {
    private readonly float _radius;
    private readonly float _radiusThickness;
    private readonly Rotation _arc;

    public SphereEmission(float radius, float radiusThickness, Rotation arc) {
        _radius = radius;
        _radiusThickness = radiusThickness;
        _arc = arc;
    }

    public Vector2 GetSpawnPosition() {
        throw new NotImplementedException();
    }

    public Vector2 GetSpawnDirection() {
        throw new NotImplementedException();
    }
}