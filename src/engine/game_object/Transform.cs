using Worms.engine.data;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class Transform : Component {
    public delegate void GameObjectInstantiate(GameObject gameObject);
    public static event GameObjectInstantiate? GameObjectInstantiateEvent;
    
    public Transform? Parent { get; }

    public Vector2 LocalPosition {
        get => _localPosition;
        set {
            if (_localPosition == value) {
                return;
            }
            _localPosition = value;
            SetDirty();
        }
    }
    public Rotation LocalRotation {
        get => _localRotation;
        set {
            if (_localRotation == value) {
                return;
            }
            _localRotation = value;
            SetDirty();
        }
    }
    public Vector2 LocalScale {
        get => _localScale;
        set {
            if (_localScale == value) {
                return;
            }
            _localScale = value;
            SetDirty();
        }
    }

    public Vector2 Position {
        get => Parent!.LocalToWorldMatrix.ConvertPoint(LocalPosition);
        set => LocalPosition = Parent!.WorldToLocalMatrix.ConvertPoint(value);
    }

    public Rotation Rotation {
        get => Parent == null ? LocalRotation : Parent.Rotation + LocalRotation;
        set => LocalRotation = value - Parent?.Rotation ?? Rotation.FromDegrees(0);
    }

    public Vector2 Scale {
        get => Parent == null ? LocalScale : Parent.Scale * LocalScale;
        set => LocalScale = value / Parent?.Scale ?? Vector2.One();
    }

    public TransformationMatrix LocalToWorldMatrix {
        get {
            if (!_isDirty) {
                return _localToWorldMatrix;
            }
            
            TransformationMatrix localToParentMatrix = TransformationMatrix.CreateLocalToParentMatrix(LocalPosition, LocalRotation, LocalScale);
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
    private Vector2 _localPosition;
    private Rotation _localRotation;
    private Vector2 _localScale;
    private TransformationMatrix _localToWorldMatrix = TransformationMatrix.Identity();
    private TransformationMatrix _worldToLocalMatrix = TransformationMatrix.Identity();
    private bool _isDirty = true;
    private bool _isInverseDirty = true;

    public Transform(Transform? parent, Vector2 localPosition, Rotation localRotation, Vector2 localScale) {
        Parent = parent;
        LocalPosition = localPosition;
        LocalRotation = localRotation;
        LocalScale = localScale;
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
        return $"ParentName: {parentName}, LocalPosition: {LocalPosition}, WorldPosition: {Position}, LocalRotation: {LocalRotation}, WorldRotation: {Rotation}, LocalScale: {LocalScale}, WorldScale: {Scale}";
    }
}