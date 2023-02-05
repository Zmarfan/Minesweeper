namespace Worms.engine.data; 

public struct Vector2Int {
    public int x = 0;
    public int y = 0;

    public int SqrMagnitude => x * x + y * y;

    public static Vector2Int Zero() {
        return new Vector2Int(0, 0);
    }
    
    public static Vector2Int One() {
        return new Vector2Int(1, 1);
    }

    public static Vector2Int Up() {
        return new Vector2Int(0, 1);
    }
    
    public static Vector2Int Down() {
        return new Vector2Int(0, -1);
    }
        
    public static Vector2Int Left() {
        return new Vector2Int(-1, 0);
    }
    
    public static Vector2Int Right() {
        return new Vector2Int(1, 0);
    }

    public static int Dot(Vector2Int lhs, Vector2Int rhs) {
        return lhs.x * rhs.x + lhs.y * rhs.y;
    }
    
    public static int Cross(Vector2Int lhs, Vector2Int rhs) {
        return lhs.x * rhs.y - rhs.x * lhs.y;
    }

    public Vector2Int(int x, int y) {
        this.x = x;
        this.y = y;
    }
    
    public override string ToString() {
        return $"x: {x}, y: {y}";
    }

    public static Vector2Int operator +(Vector2Int a, Vector2Int b) {
        return new Vector2Int(a.x + b.x, a.y + b.y);
    }

    public static Vector2Int operator -(Vector2Int a, Vector2Int b) {
        return new Vector2Int(a.x - b.x, a.y - b.y);
    }

    public static Vector2Int operator *(Vector2Int a, Vector2Int b) {
        return new Vector2Int(a.x * b.x, a.y * b.y);
    }

    public static Vector2Int operator /(Vector2Int a, Vector2Int b) {
        return new Vector2Int(a.x / b.x, a.y / b.y);
    }

    public static Vector2Int operator -(Vector2Int a) {
        return new Vector2Int(0 - a.x, 0 - a.y);
    }

    public static Vector2Int operator *(Vector2Int a, int d) {
        return new Vector2Int(a.x * d, a.y * d);
    }

    public static Vector2Int operator *(int d, Vector2Int a) {
        return new Vector2Int(a.x * d, a.y * d);
    }

    public static Vector2Int operator /(Vector2Int a, int d) {
        return new Vector2Int(a.x / d, a.y / d);
    }

    public static bool operator ==(Vector2Int lhs, Vector2Int rhs) {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    public static bool operator !=(Vector2Int lhs, Vector2Int rhs) {
        return !(lhs == rhs);
    }
}