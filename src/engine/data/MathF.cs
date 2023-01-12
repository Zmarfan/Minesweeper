namespace Worms.engine.data; 

public static class MathF {
    private const float EPSILON = 9.99999944E-6f;
    
    public static float DegreeToRadian(float angle) {
        return (float)(Math.PI * angle / 180);
    }

    public static float RadiansToDegree(float radians) {
        return (float)(180 / Math.PI * radians);
    }

    public static float CloseToIntToInt(float value) {
        int nearestInt = (int)Math.Round(value, 0);
        return value - nearestInt < EPSILON && value - nearestInt > -EPSILON ? nearestInt : value;
    }
}