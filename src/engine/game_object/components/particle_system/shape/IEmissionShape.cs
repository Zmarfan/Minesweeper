using GameEngine.engine.data;

namespace GameEngine.engine.game_object.components.particle_system.shape; 

public interface IEmissionShape {
    Tuple<Vector2, Vector2> GetSpawnPositionAndDirection(Random random);
}