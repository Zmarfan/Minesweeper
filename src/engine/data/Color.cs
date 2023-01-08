namespace Worms.engine.data; 

public struct Color {
    public float r;
    public float g;
    public float b;
    public float a;
    
    public byte Rbyte => (byte)(r * byte.MaxValue);
    public byte Gbyte => (byte)(g * byte.MaxValue);
    public byte Bbyte => (byte)(b * byte.MaxValue);
    public byte Abyte => (byte)(a * byte.MaxValue);

    public static Color White() {
        return new Color(1, 1, 1);
    }
    
    public static Color Black() {
        return new Color(0, 0, 0, 1);
    }
    
    public Color(float r, float g, float b, float a) {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public Color(float r, float g, float b) {
        this.r = r;
        this.g = g;
        this.b = b;
        a = 1f;
    }

    public static Color Lerp(Color a, Color b, float t) {
        t = Math.Clamp(t, 0, 1);
        return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
    }
    
    public static Color LerpUnclamped(Color a, Color b, float t) {
        return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
    }
    
    public override string ToString() {
        return $"r: {r}, g: {g}, b: {b}, a: {a}";
    }
}