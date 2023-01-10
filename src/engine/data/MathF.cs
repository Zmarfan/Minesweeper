namespace Worms.engine.data; 

public static class MathF {
    private const float EPSILON = 9.99999944E-11f;
    
    public static float DegreeToRadian(float angle) {
        return (float)(Math.PI * angle / 180);
    }

    public static float CloseToZeroToZero(float value) {
        return value < EPSILON ? 0 : value;
    }
}