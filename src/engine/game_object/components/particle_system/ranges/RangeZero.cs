using Worms.engine.helper;

namespace Worms.engine.game_object.components.particle_system.ranges; 

public readonly struct RangeZero {
    private readonly float _value1;
    private readonly float _value2;

    public RangeZero(float value1, float value2) {
        if (value1 < 0 || value2 < 0) {
            throw new ArgumentException("The values provided can not be lower than zero");
        }

        _value1 = value1;
        _value2 = value2;
    }

    public RangeZero(float constant) {
        if (constant < 0) {
            throw new ArgumentException("The constant provided can not be lower than zero");
        }
        _value1 = constant;
        _value2 = constant;
    }

    public static RangeZero Zero() {
        return new RangeZero(0);
    }

    public float GetRandom(Random random) {
        return RandomUtil.GetRandomValueBetweenTwoValues(random, _value1, _value2);
    }
}