namespace Worms.engine.helper; 

public static class RandomUtil {
    public static float GetRandomValueBetweenTwoValues(Random random, float v1, float v2) {
        float max = Math.Max(v1, v2);
        float min = Math.Min(v1, v2);
        return (float)(random.NextDouble() * (max - min) + min);
    }
}