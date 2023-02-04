namespace Worms.engine.data; 

public struct Color {
    public static readonly Color BLACK = new(0, 0, 0);
    public static readonly Color WHITE = new(1, 1, 1);
    public static readonly Color RED = new(1, 0, 0);
    public static readonly Color GREEN = new(0, 1, 0);
    public static readonly Color BLUE = new(0, 0, 1);
    public static readonly Color YELLOW = new(255, 216, 0, 255);
    public static readonly Color TRANSPARENT = new(0, 0, 0, 0);
    
    public float r;
    public float g;
    public float b;
    public float a;
    
    public byte Rbyte => (byte)(r * byte.MaxValue);
    public byte Gbyte => (byte)(g * byte.MaxValue);
    public byte Bbyte => (byte)(b * byte.MaxValue);
    public byte Abyte => (byte)(a * byte.MaxValue);

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
    
    public Color(byte r, byte g, byte b, byte a) {
        this.r = (float)r / byte.MaxValue;
        this.g = (float)g / byte.MaxValue;
        this.b = (float)b / byte.MaxValue;
        this.a = (float)a / byte.MaxValue;
    }

    public bool IsOpaque => Abyte == byte.MaxValue;
    
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
    
    public static bool operator ==(Color c1, Color c2) {
        return c1.Rbyte == c2.Rbyte && c1.Gbyte == c2.Gbyte && c1.Bbyte == c2.Bbyte && c1.Abyte == c2.Abyte;
    }

    public static bool operator !=(Color c1, Color c2) {
        return !(c1 == c2);
    }
}