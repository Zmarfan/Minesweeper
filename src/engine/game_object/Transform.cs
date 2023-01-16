using Worms.engine.data;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class Transform : Component {
    public delegate void GameObjectInstantiate(GameObject gameObject);
    public static event GameObjectInstantiate? GameObjectInstantiateEvent;
    
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
    public Vector2 Scale {
        get => _scale;
        set {
            _scale = value;
            SetDirty();
        }
    }

    public Vector2 WorldPosition => LocalToWorldMatrix.ConvertPoint(Vector2.Zero());
    public Rotation WorldRotation => Parent == null ? Rotation : Parent.WorldRotation + Rotation;
    public Vector2 WorldScale => Parent == null ? Scale : Parent.WorldScale * Scale;

    public TransformationMatrix LocalToWorldMatrix {
        get {
            if (!_isDirty) {
                return _localToWorldMatrix;
            }
            
            TransformationMatrix localToParentMatrix = TransformationMatrix.CreateLocalToParentMatrix(Position, Rotation, Scale);
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

    public Transform(Transform? parent, Vector2 position, Rotation rotation, Vector2 scale) {
        Parent = parent;
        Position = position;
        Rotation = rotation;
        Scale = scale;
        parent?.SetChild(this);
    }

    public Transform GetRoot() {
        return Transform.Parent == null ? this : Transform.Parent.GetRoot();
    }

    public GameObjectBuilder AddSibling(string name) {
        return GameObjectBuilder.Builder(name, Parent?.gameObject!);
    }
    
    public GameObjectBuilder AddChild(string name) {
        return GameObjectBuilder.Builder(name, gameObject);
    }

    public GameObject Instantiate(GameObjectBuilder builder) {
        builder.SetParent(this);
        GameObject go = builder.Build();
        GameObjectInstantiateEvent?.Invoke(go);
        return go;
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