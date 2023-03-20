using Newtonsoft.Json;

namespace GameEngine.engine.data; 

public struct Rotation {
    public float Degree {
        get => _degree;
        set => _degree = value % 360;
    }
    private float _degree;
    [JsonIgnore]
    public float Radians => MathF.DegreeToRadian(_degree);

    public static Rotation Identity() {
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
        return new Rotation(a._degree + (b._degree - a._degree) * t);
    }
    
    public static Rotation Mirror(Rotation a) {
        return new Rotation(a._degree + 180);
    }

    public static Rotation FromDegrees(float degrees) {
        return new Rotation(degrees);
    }
    
    public static Rotation FromRadians(float radians) {
        return new Rotation(MathF.RadiansToDegree(radians));
    }
    
    private Rotation(float degree) {
        _degree = MathF.Modulo(degree, 360);
    }
    
    public override string ToString() {
        return $"rotation: {Degree}°";
    }
    
    public static Rotation operator +(Rotation a, Rotation b) {
        return new Rotation(a._degree + b._degree);
    }

    public static Rotation operator -(Rotation a, Rotation b) {
        return new Rotation(a._degree - b._degree);
    }
    
    public static Rotation operator -(Rotation a) {
        return new Rotation(0f - a._degree);
    }

    public static Rotation operator *(Rotation a, float d) {
        return new Rotation(a._degree * d);
    }

    public static Rotation operator *(float d, Rotation a) {
        return new Rotation(a._degree * d);
    }

    public static Rotation operator +(Rotation a, float d) {
        return new Rotation(a._degree + d);
    }

    public static Rotation operator +(float d, Rotation a) {
        return new Rotation(a._degree + d);
    }
    
    public static Rotation operator -(Rotation a, float d) {
        return new Rotation(a._degree - d);
    }

    public static Rotation operator -(float d, Rotation a) {
        return new Rotation(a._degree - d);
    }
    
    public static bool operator ==(Rotation lhs, Rotation rhs) {
        float num = lhs._degree - rhs._degree;
        return num * num < 9.99999944E-11f;
    }

    public static bool operator !=(Rotation lhs, Rotation rhs) {
        return !(lhs == rhs);
    }
    
    public override bool Equals(object? obj) {
        return obj is Rotation rotation && rotation == this;
    }

    public override int GetHashCode() {
        return _degree.GetHashCode();
    }
}