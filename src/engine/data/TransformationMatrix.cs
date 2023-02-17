namespace Worms.engine.data; 

public readonly struct TransformationMatrix {
    private static readonly float[,] IDENTITY_VALUES = {
        { 1f, 0f, 0f },
        { 0f, 1f, 0f },
        { 0f, 0f, 1f }
    };
    
    private readonly float[,] _values = IDENTITY_VALUES;

    private TransformationMatrix(float[,] values) {
        _values = values;
    }
    
    public static TransformationMatrix Identity() {
        return new TransformationMatrix(IDENTITY_VALUES);
    }

    public static TransformationMatrix CreateWorldToScreenMatrix(Vector2 position, Vector2 scale) {
        return Scale(scale) * Translate(position) * RotateCameraYAxis();
    }
    
    public static TransformationMatrix CreateLocalToParentMatrix(Vector2 position, Rotation rotation, Vector2 scale) {
        return Translate(position) * Rotate(rotation) * Scale(scale);
    }
    
    public Vector2 GetTranslation() {
        return new Vector2(_values[2, 0], _values[2, 1]);
    }

    public Vector2 GetScale() {
        return new Vector2(
            (float)Math.Sqrt(_values[0, 0] * _values[0, 0] + _values[0, 1] * _values[0, 1]),
            (float)Math.Sqrt(_values[1, 0] * _values[1, 0] + _values[1, 1] * _values[1, 1])
        );
    }

    public Rotation GetRotation() {
        Rotation rotation = Rotation.FromRadians((float)Math.Acos(_values[0, 0] / GetScale().x));
        rotation = Math.Sign(_values[0, 1]) == 1 ? Rotation.FromDegrees(360 - rotation.Degree) : rotation;
        return rotation;
    }

    public Vector2 ConvertPoint(Vector2 p) {
        float x = MathF.CloseToIntToInt(_values[0, 0] * p.x + _values[1, 0] * p.y + _values[2, 0]);
        float y = MathF.CloseToIntToInt(_values[0, 1] * p.x + _values[1, 1] * p.y + _values[2, 1]);
        return new Vector2(x, y);
    }
    
    public Vector2 ConvertVector(Vector2 v) {
        float x = MathF.CloseToIntToInt(_values[0, 0] * v.x + _values[1, 0] * v.y);
        float y = MathF.CloseToIntToInt(_values[0, 1] * v.x + _values[1, 1] * v.y);
        return new Vector2(x, y);
    }

    public TransformationMatrix Inverse() {
        float determinant = 1 / (
            _values[0, 0] * (_values[1, 1] * _values[2, 2] - _values[2, 1] * _values[1, 2])
            - _values[1, 0] * (_values[0, 1] * _values[2, 2] - _values[2, 1] * _values[0, 2])
            - _values[2, 0] * (_values[0, 1] * _values[1, 2] - _values[1, 1] * _values[0, 2])
        );
        float[,] values = {
            {
                _values[1, 1] * _values[2, 2] - _values[2, 1] * _values[1, 2], 
                _values[2, 1] * _values[0, 2] - _values[0, 1] * _values[2, 2],
                _values[0, 1] * _values[1, 2] - _values[1, 1] * _values[0, 2]
            },
            {
                _values[2, 0] * _values[1, 2] - _values[1, 0] * _values[2, 2],
                _values[0, 0] * _values[2, 2] - _values[2, 0] * _values[0, 2],
                _values[1, 0] * _values[0, 2] - _values[0, 0] * _values[1, 2]
            },
            {
                _values[1, 0] * _values[2, 1] - _values[2, 0] * _values[1, 1],
                _values[2, 0] * _values[0, 1] - _values[0, 0] * _values[2, 1],
                _values[0, 0] * _values[1, 1] - _values[1, 0] * _values[0, 1]
            }
        };
        return determinant * new TransformationMatrix(values);
    }

    public static TransformationMatrix Translate(Vector2 position) {
        return new TransformationMatrix(new[,] {
            { 1f, 0f, 0f }, 
            { 0f, 1f, 0f }, 
            { position.x, position.y, 1f }
        });
    }
    
    private static TransformationMatrix Rotate(Rotation rotation) {
        float cos = MathF.CloseToIntToInt((float)Math.Cos(rotation.Radians));
        float sin = MathF.CloseToIntToInt((float)Math.Sin(rotation.Radians));
        return new TransformationMatrix(new[,] {
            { cos, -sin, 0f }, 
            { sin, cos, 0f }, 
            { 0f, 0f, 1f }
        });
    }
    
    private static TransformationMatrix RotateCameraYAxis() {
        float cos = MathF.CloseToIntToInt((float)Math.Cos(Rotation.FromDegrees(180).Radians));
        float sin = MathF.CloseToIntToInt((float)Math.Sin(Rotation.FromDegrees(180).Radians));
        return new TransformationMatrix(new[,] {
            { 1, -sin, 0f }, 
            { 0, cos, 0f }, 
            { 0f, 0f, 1f }
        });
    }

    private static TransformationMatrix Scale(Vector2 scale) {
        return new TransformationMatrix(new[,] { { scale.x, 0f, 0f }, { 0f, scale.y, 0f }, { 0f, 0f, 1f } });
    }
    
    public static TransformationMatrix operator *(TransformationMatrix a, TransformationMatrix b) {
        float[,] values = new float[3, 3];
        for (int x = 0; x < a._values.GetLength(0); x++) {
            for (int y = 0; y < a._values.GetLength(1); y++) {
                values[x, y] = MathF.CloseToIntToInt(a._values[0, y] * b._values[x, 0] + a._values[1, y] * b._values[x, 1] + a._values[2, y] * b._values[x, 2]);
            }
        }

        return new TransformationMatrix(values);
    }
    
    public static TransformationMatrix operator *(float value, TransformationMatrix matrix) {
        float[,] values = new float[3, 3];
        for (int x = 0; x < matrix._values.GetLength(0); x++) {
            for (int y = 0; y < matrix._values.GetLength(1); y++) {
                values[x, y] = MathF.CloseToIntToInt(matrix._values[x, y] * value);
            }
        }

        return new TransformationMatrix(values);
    }

    public override string ToString() {
        return $"{_values[0, 0]}, {_values[1, 0]}, {_values[2, 0]}\n{_values[0, 1]}, {_values[1, 1]}, {_values[2, 1]}\n{_values[0, 2]}, {_values[1, 2]}, {_values[2, 2]}";
    }
}