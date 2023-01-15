using Worms.engine.data;

namespace Worms.engine.game_object.components.particle_system.shape; 

public class LineEmission : IEmissionShape {
    private readonly float _radius;

    public LineEmission(float radius) {
        _radius = radius;
    }

    public Vector2 GetSpawnPosition() {
        throw new NotImplementedException();
    }

    public Vector2 GetSpawnDirection() {
        throw new NotImplementedException();
    }
}