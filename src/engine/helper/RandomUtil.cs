namespace Worms.engine.helper; 

public static class RandomUtil {
    private static readonly Random RANDOM = new();
    
    public static float GetRandomValueBetweenTwoValues(float v1, float v2) {
        return GetRandomValueBetweenTwoValues(RANDOM, v1, v2);
    }
    
    public static float GetRandomValueBetweenTwoValues(Random random, float v1, float v2) {
        float max = Math.Max(v1, v2);
        float min = Math.Min(v1, v2);
        return (float)(random.NextDouble() * (max - min) + min);
    }
    
    public static int GetRandomZeroToMax(int max) {
        return GetRandomZeroToMax(RANDOM, max);
    }
    
    public static int GetRandomZeroToMax(Random random, int max) {
        return random.Next(max);
    }

    public static bool RandomBool() {
        return RandomBool(RANDOM);
    }
    
    public static bool RandomBool(Random random) {
        return random.Next(2) == 0;
    }
}