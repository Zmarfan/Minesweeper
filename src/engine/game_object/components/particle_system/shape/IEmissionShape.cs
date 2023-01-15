using Worms.engine.data;

namespace Worms.engine.game_object.components.particle_system.shape; 

public interface IEmissionShape {
    Vector2 GetSpawnPosition();
    Vector2 GetSpawnDirection();
}