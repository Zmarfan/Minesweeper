namespace Worms.engine.data; 

public struct Vector2 {
    public float x = 0;
    public float y = 0;

    public Vector2 Normalized {
        get {
            Vector2 copy = new(x, y);
            copy.Normalize();
            return copy;
        }
    }
    public float Magnitude => (float)Math.Sqrt(x * x + y * y);
    public float SqrMagnitude => x * x + y * y;

    public static Vector2 Zero() {
        return new Vector2(0, 0);
    }
    
    public static Vector2 One() {
        return new Vector2(1, 1);
    }

    public static Vector2 Up() {
        return new Vector2(0, 1);
    }
    
    public static Vector2 Down() {
        return new Vector2(0, -1);
    }
        
    public static Vector2 Left() {
        return new Vector2(-1, 0);
    }
    
    public static Vector2 Right() {
        return new Vector2(1, 0);
    }
    
    public static Vector2 Lerp(Vector2 a, Vector2 b, float t) {
        t = Math.Clamp(t, 0, 1);
        return new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
    }
    
    public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta) {
        float num = target.x - current.x;
        float num2 = target.y - current.y;
        float num3 = num * num + num2 * num2;
        if (num3 == 0f || (maxDistanceDelta >= 0f && num3 <= maxDistanceDelta * maxDistanceDelta))
        {
            return target;
        }

        float num4 = (float)Math.Sqrt(num3);
        return new Vector2(current.x + num / num4 * maxDistanceDelta, current.y + num2 / num4 * maxDistanceDelta);
    }
    
    public static Vector2 Reflect(Vector2 inDirection, Vector2 inNormal) {
        float num = -2f * Dot(inNormal, inDirection);
        return new Vector2(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y);
    }
    
    public static float Dot(Vector2 lhs, Vector2 rhs) {
        return lhs.x * rhs.x + lhs.y * rhs.y;
    }
    
    public static float Cross(Vector2 lhs, Vector2 rhs) {
        return lhs.x * rhs.y - rhs.x * lhs.y;
    }
    
    public static float Distance(Vector2 a, Vector2 b) {
        float num = a.x - b.x;
        float num2 = a.y - b.y;
        return (float)Math.Sqrt(num * num + num2 * num2);
    }
    
    public static Vector2 ClampMagnitude(Vector2 vector, float maxLength) {
        float num = vector.SqrMagnitude;
        if (num > maxLength * maxLength)
        {
            float num2 = (float)Math.Sqrt(num);
            float num3 = vector.x / num2;
            float num4 = vector.y / num2;
            return new Vector2(num3 * maxLength, num4 * maxLength);
        }

        return vector;
    }
    
    public static Vector2 RotatePointAroundPoint(Vector2 point, Vector2 pivot, float angle) {
        double radians = Math.PI * angle / 180;
        float s = (float)Math.Sin(radians);
        float c = (float)Math.Cos(radians);

        point -= pivot;
        point = new Vector2(point.x * c - point.y * s, point.x * s + point.y * c) + pivot;

        return point;
    }
    
    public Vector2(float x, float y) {
        this.x = x;
        this.y = y;
    }

    public void Normalize() {
        float num = Magnitude;
        if (num > 1E-05f)
        {
            x /= num;
            y /= num;
        }
        else {
            x = 0;
            y = 0;
        }
    }

    public void Abs() {
        x = Math.Abs(x);
        y = Math.Abs(y);
    }
    
    public static Vector2 RotatePoint(Vector2 center, Rotation rotation, Vector2 point) {
        float s = (float)Math.Sin(-rotation.Radians);
        float c = (float)Math.Cos(-rotation.Radians);

        point -= center;

        Vector2 rotated = new(point.x * c - point.y * s, point.x * s + point.y * c);

        return rotated + center;
    }
    
    public override string ToString() {
        return $"x: {x}, y: {y}";
    }

    public static Vector2 operator +(Vector2 a, Vector2 b) {
        return new Vector2(a.x + b.x, a.y + b.y);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b) {
        return new Vector2(a.x - b.x, a.y - b.y);
    }

    public static Vector2 operator *(Vector2 a, Vector2 b) {
        return new Vector2(a.x * b.x, a.y * b.y);
    }

    public static Vector2 operator /(Vector2 a, Vector2 b) {
        return new Vector2(a.x / b.x, a.y / b.y);
    }

    public static Vector2 operator -(Vector2 a) {
        return new Vector2(0f - a.x, 0f - a.y);
    }

    public static Vector2 operator *(Vector2 a, float d) {
        return new Vector2(a.x * d, a.y * d);
    }

    public static Vector2 operator *(float d, Vector2 a) {
        return new Vector2(a.x * d, a.y * d);
    }

    public static Vector2 operator /(Vector2 a, float d) {
        return new Vector2(a.x / d, a.y / d);
    }

    public static bool operator ==(Vector2 lhs, Vector2 rhs) {
        float num = lhs.x - rhs.x;
        float num2 = lhs.y - rhs.y;
        return num * num + num2 * num2 < 9.99999944E-11f;
    }

    public static bool operator !=(Vector2 lhs, Vector2 rhs) {
        return !(lhs == rhs);
    }
    
    public override bool Equals(object? obj) {
        return obj is Vector2 vector && vector == this;
    }

    public override int GetHashCode() {
        return (x, y).GetHashCode();
    }
}