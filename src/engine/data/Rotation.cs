namespace Worms.engine.data; 

public struct Rotation {
    public float Value {
        get => _value;
        set => _value = value % 360;
    }
    private float _value;

    public static Rotation Normal() {
        return new Rotation(0);
    }
    
    public static Rotation UpsideDown() {
        return new Rotation(180);
    }

    public static Rotation Clockwise() {
        return new Rotation(90);
    }
    
    public static Rotation CounterClockwise() {
        return new Rotation(-90);
    }
    
    public static Rotation Lerp(Rotation a, Rotation b, float t) {
        t = Math.Clamp(t, 0, 1);
        return new Rotation(a._value + (b._value - a._value) * t);
    }
    
    public static Rotation Mirror(Rotation a) {
        return new Rotation(a._value + 180);
    }
    
    public Rotation(float value) {
        _value = value % 360;
    }
    
    public override string ToString() {
        return $"rotation: {Value}°";
    }
    
    public static Rotation operator +(Rotation a, Rotation b) {
        return new Rotation(a._value + b._value);
    }

    public static Rotation operator -(Rotation a, Rotation b) {
        return new Rotation(a._value - b._value);
    }
    
    public static Rotation operator -(Rotation a) {
        return new Rotation(0f - a._value);
    }

    public static Rotation operator *(Rotation a, float d) {
        return new Rotation(a._value * d);
    }

    public static Rotation operator *(float d, Rotation a) {
        return new Rotation(a._value * d);
    }

    public static Rotation operator +(Rotation a, float d) {
        return new Rotation(a._value + d);
    }

    public static Rotation operator +(float d, Rotation a) {
        return new Rotation(a._value + d);
    }
    
    public static Rotation operator -(Rotation a, float d) {
        return new Rotation(a._value - d);
    }

    public static Rotation operator -(float d, Rotation a) {
        return new Rotation(a._value - d);
    }
    
    public static bool operator ==(Rotation lhs, Rotation rhs) {
        float num = lhs._value - rhs._value;
        return num * num < 9.99999944E-11f;
    }

    public static bool operator !=(Rotation lhs, Rotation rhs) {
        return !(lhs == rhs);
    }
}