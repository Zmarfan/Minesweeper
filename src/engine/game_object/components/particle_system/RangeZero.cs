namespace Worms.engine.game_object.components.particle_system; 

public readonly struct RangeZero {
    public readonly float min;
    public readonly float max;

    public RangeZero(float value1, float value2) {
        if (min < 0 || max < 0) {
            throw new ArgumentException("The values provided can not be lower than zero");
        }
        
        min = Math.Min(value1, value2);
        max = Math.Max(value1, value2);
    }

    public RangeZero(float constant) {
        if (constant < 0) {
            throw new ArgumentException("The constant provided can not be lower than zero");
        }
        min = constant;
        max = constant;
    }

    public static RangeZero Zero() {
        return new RangeZero(0);
    }

    public float GetRandom(Random random) {
        return (float)(random.NextDouble() * (max - min) + min);
    }
}