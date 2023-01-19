using Worms.engine.core;
using Worms.engine.data;

namespace Worms.engine.camera; 

public abstract class Camera {
    public Vector2 Position {
        get => _position;
        set {
            SetDirty();
            _position = value;
        }
    }
    public float Size {
        get => _size;
        set {
            SetDirty();
            _size = value;
        }
    }
    public Color defaultDrawColor = Color.BLACK;

    public TransformationMatrix WorldToScreenMatrix {
        get {
            if (_oldScreenHeight != _settings.height || _oldScreenWidth != _settings.width) {
                SetDirty();
            }
            if (!_isDirty) {
                return _worldToScreenMatrix;
            }

            _isDirty = false;
            _worldToScreenMatrix = TransformationMatrix.CreateWorldToScreenMatrix(
                new Vector2(_oldScreenWidth / 2f - _position.x, _oldScreenHeight / 2f + _position.y),
                new Vector2(1 / _size, 1 / _size)
            );
            return _worldToScreenMatrix;
        }
    }

    public TransformationMatrix ScreenToWorldMatrix {
        get {
            if (_oldScreenHeight != _settings.height || _oldScreenWidth != _settings.width) {
                SetDirty();
            }
            if (!_isInverseDirty) {
                return _screenToWorldMatrix;
            }

            _isInverseDirty = false;
            _screenToWorldMatrix = WorldToScreenMatrix.Inverse();
            return _screenToWorldMatrix;
        }
    }

    private Vector2 _position = Vector2.Zero();
    private float _size = 1;
    private TransformationMatrix _worldToScreenMatrix = TransformationMatrix.Identity();
    private TransformationMatrix _screenToWorldMatrix = TransformationMatrix.Identity();
    private bool _isDirty = true;
    private bool _isInverseDirty = true;
    private GameSettings _settings = null!;
    private int _oldScreenWidth;
    private int _oldScreenHeight;

    public void Init(GameSettings settings) {
        _settings = settings;
        SetDirty();
    }

    public virtual void Awake() {
    }

    public virtual void Update(float deltaTime) {
    }
    
    private void SetDirty() {
        _isDirty = true;
        _isInverseDirty = true;
        _oldScreenWidth = _settings.width;
        _oldScreenHeight = _settings.height;
    }
}