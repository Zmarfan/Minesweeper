using GameEngine.engine.helper;

namespace GameEngine.engine.game_object.components.particle_system.ranges; 

public readonly struct Range {
    private readonly float _value1;
    private readonly float _value2;

    public Range(float value1, float value2) {
        _value1 = value1;
        _value2 = value2;
    }

    public Range(float constant) {
        _value1 = constant;
        _value2 = constant;
    }

    public float GetRandom(Random random) {
        return RandomUtil.GetRandomValueBetweenTwoValues(random, _value1, _value2);
    }
}