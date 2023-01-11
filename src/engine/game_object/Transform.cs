using Worms.engine.data;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class Transform : Component {
    public delegate void TransformUpdate();
    public static event TransformUpdate? TransformHierarchyEvent;
    
    public Transform? Parent { get; }

    public Vector2 Position {
        get => _position;
        set {
            _position = value;
            SetDirty();
        }
    }
    public Rotation Rotation {
        get => _rotation;
        set {
            _rotation = value;
            SetDirty();
        }
    }
    public float Scale {
        get => _scale.x;
        set {
            _scale = new Vector2(value, value);
            SetDirty();
        }
    }

    public Vector2 WorldPosition => LocalToWorldMatrix.ConvertPoint(Vector2.Zero());
    
    public Rotation WorldRotation {
        get {
            if (Parent == null) {
                return Rotation;
            }

            return Parent.WorldRotation + Rotation;
        }
    }
    
    public float WorldScale {
        get {
            if (Parent == null) {
                return Scale;
            }

            return Parent.WorldScale * Scale;
        }
    }
    
    public TransformationMatrix LocalToWorldMatrix {
        get {
            if (!_isDirty) {
                return _localToWorldMatrix;
            }
            
            TransformationMatrix localToParentMatrix = TransformationMatrix.CreateLocalToParentMatrix(
                Position, 
                Rotation, 
                new Vector2(Scale, Scale)
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

    public Transform(Transform? parent, Vector2 position, Rotation rotation, float scale) {
        Parent = parent;
        Position = position;
        Rotation = rotation;
        Scale = scale;
        parent?.SetChild(this);
    }

    public Transform GetRoot() {
        return Transform.Parent == null ? this : Transform.Parent.GetRoot();
    }
    
    public Transform GetParent() {
        return Transform.Parent!;
    }
    
    public GameObjectBuilder AddSibling(string name) {
        return GameObjectBuilder.Builder(name, Parent?.gameObject!);
    }
    
    public GameObjectBuilder AddChild(string name) {
        return GameObjectBuilder.Builder(name, gameObject);
    }

    public void AddChild(GameObjectBuilder builder) {
        builder.SetParent(this);
        builder.Build();
        TransformHierarchyEvent?.Invoke();
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
        return $"ParentName: {parentName}, LocalPosition: {Position}, WorldPosition: {WorldPosition}, LocalRotation: {Rotation}, WorldRotation: {WorldRotation}, LocalScale: {Scale}, WorldScale: {WorldScale}";
    }
}