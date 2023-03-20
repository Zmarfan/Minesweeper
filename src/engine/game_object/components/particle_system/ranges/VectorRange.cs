using GameEngine.engine.data;
using GameEngine.engine.helper;

namespace GameEngine.engine.game_object.components.particle_system.ranges; 

public readonly struct VectorRange {
    private readonly Vector2 _vector1;
    private readonly Vector2 _vector2;

    public VectorRange(Vector2 vector1, Vector2 vector2) {
        _vector1 = vector1;
        _vector2 = vector2;
    }

    public VectorRange(Vector2 vector) {
        _vector1 = vector;
        _vector2 = vector;
    }

    public Vector2 GetRandom(Random random) {
        return new Vector2(
            RandomUtil.GetRandomValueBetweenTwoValues(random, _vector1.x, _vector2.x),
            RandomUtil.GetRandomValueBetweenTwoValues(random, _vector1.y, _vector2.y)
        );
    }
}