using Worms.engine.data;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class Transform : Component {
    public Transform? Parent { get; }

    public Vector2 LocalPosition {
        get => _position;
        set {
            _position = value;
            SetDirty();
        }
    }
    public Rotation LocalRotation {
        get => _rotation;
        set {
            _rotation = value;
            SetDirty();
        }
    }
    public float LocalScale {
        get => _scale.x;
        set {
            _scale = new Vector2(value, value);
            SetDirty();
        }
    }

    public TransformationMatrix LocalToWorldMatrix {
        get {
            if (!_isDirty) {
                return _localToWorldMatrix;
            }
            
            TransformationMatrix localToParentMatrix = TransformationMatrix.CreateLocalToParentMatrix(
                LocalPosition, 
                LocalRotation, 
                new Vector2(LocalScale, LocalScale)
            );
            if (Parent == null) {
                _localToWorldMatrix = localToParentMatrix;
            } 
            else {
                _localToWorldMatrix = Parent.LocalToWorldMatrix * localToParentMatrix;
            }
            
            _isDirty = false;
            return _localToWorldMatrix;
        }
    }

    public TransformationMatrix WorldToLocalMatrix {
        get {
            if (_isInverseDirty) {
                _worldToLocalMatrix = LocalToWorldMatrix.Inverse();
                _isInverseDirty = false;
            }
            return _worldToLocalMatrix;
        }
    }
    
    public readonly List<Transform> children = new();
    private Vector2 _position;
    private Rotation _rotation;
    private Vector2 _scale;
    private TransformationMatrix _localToWorldMatrix = TransformationMatrix.Identity();
    private TransformationMatrix _worldToLocalMatrix = TransformationMatrix.Identity();
    private bool _isDirty = true;
    private bool _isInverseDirty = true;

    public Vector2 WorldPosition => LocalToWorldMatrix.ConvertPoint(Vector2.Zero());
    
    public Rotation WorldRotation {
        get {
            if (Parent == null) {
                return LocalRotation;
            }

            return Parent.WorldRotation + LocalRotation;
        }
    }
    
    public float WorldScale {
        get {
            if (Parent == null) {
                return LocalScale;
            }

            return Parent.WorldScale * LocalScale;
        }
    }
    
    public Transform(Transform? parent, Vector2 localPosition, Rotation localRotation, float localScale) {
        Parent = parent;
        LocalPosition = localPosition;
        LocalRotation = localRotation;
        LocalScale = localScale;
        parent?.SetChild(this);
    }

    private void SetChild(Transform child) {
        if (children.Contains(child)) {
            throw new ArgumentException("Can not add child to parent who already has that child");
        }
        children.Add(child);
    }

    private void SetDirty() {
        _isDirty = true;
        _isInverseDirty = true;
        foreach (Transform child in children) {
            child.SetDirty();
        }
    }

    public override string ToString() {
        string parentName = Parent?.gameObject.Name ?? "root";
        return $"ParentName: {parentName}, LocalPosition: {LocalPosition}, WorldPosition: {WorldPosition}, LocalRotation: {LocalRotation}, WorldRotation: {WorldRotation}, LocalScale: {LocalScale}, WorldScale: {WorldScale}";
    }
}