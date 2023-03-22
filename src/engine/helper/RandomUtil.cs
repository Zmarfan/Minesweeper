namespace GameEngine.engine.helper; 

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

    public static void RandomizeList<T>(ref List<T> list) {
        RandomizeList(RANDOM, ref list);
    }
    
    public static void RandomizeList<T>(Random random, ref List<T> list) {
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = random.Next(n + 1);  
            (list[k], list[n]) = (list[n], list[k]);
        }  
    }
}