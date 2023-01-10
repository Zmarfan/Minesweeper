namespace Worms.engine.data; 

public readonly struct TransformationMatrix {
    private readonly float _m00;
    private readonly float _m10;
    private readonly float _m20;
    private readonly float _m01;
    private readonly float _m11;
    private readonly float _m21;
    private const float M02 = 0f;
    private const float M12 = 0f;
    private const float M22 = 1f;

    private TransformationMatrix(float m00, float m10, float m20, float m01, float m11, float m21) {
        _m00 = m00;
        _m10 = m10;
        _m20 = m20;
        _m01 = m01;
        _m11 = m11;
        _m21 = m21;
    }
    
    public static TransformationMatrix Identity() {
        return new TransformationMatrix(1f, 0f, 0f, 0f, 1f, 0f);
    }

    public static TransformationMatrix Create(
        TransformationMatrix baseMatrix,
        Vector2 position,
        Rotation rotation,
        Vector2 scale
    ) {
        TransformationMatrix matrix = CombineMatrix(baseMatrix, CreateMatrix(position, rotation, scale));
        return matrix;
    }

    public Vector2 ConvertPoint(Vector2 p) {
        float x = _m00 * p.x + _m10 * p.y + _m20;
        float y = _m01 * p.x + _m11 * p.y + _m21;
        return new Vector2(x, y);
    }
    
    public Vector2 ConvertVector(Vector2 v) {
        float x = _m00 * v.x + _m10 * v.y;
        float y = _m01 * v.x + _m11 * v.y;
        return new Vector2(x, y);
    }

    private static TransformationMatrix CreateMatrix(Vector2 position, Rotation rotation, Vector2 scale) {
        float cos = MathF.CloseToZeroToZero((float)Math.Cos(rotation.Radians));
        float sin = MathF.CloseToZeroToZero((float)Math.Sin(rotation.Radians));
        float m00 = scale.x * cos;
        float m10 = -scale.y * sin;
        float m20 = position.x;
        float m01 = scale.x * sin;
        float m11 = scale.y * cos;
        float m21 = position.y;
        return new TransformationMatrix(m00, m10, m20, m01, m11, m21);
    }
    
    private static TransformationMatrix CombineMatrix(TransformationMatrix b, TransformationMatrix c) {
        float m00 = b._m00 * c._m00 + b._m10 * c._m01 + b._m20 * M02;
        float m10 = b._m00 * c._m10 + b._m10 * c._m11 + b._m20 * M12;
        float m20 = b._m00 * c._m20 + b._m10 * c._m21 + b._m20 * M22;
        float m01 = b._m01 * c._m00 + b._m11 * c._m01 + b._m21 * M02;
        float m11 = b._m01 * c._m10 + b._m11 * c._m11 + b._m21 * M12;
        float m21 = b._m01 * c._m20 + b._m11 * c._m21 + b._m21 * M22;
        return new TransformationMatrix(m00, m10, m20, m01, m11, m21);
    }

    public override string ToString() {
        return $"{_m00}, {_m10}, {_m20}\n{_m01}, {_m11}, {_m21}\n{M02}, {M12}, {M22}";
    }
}