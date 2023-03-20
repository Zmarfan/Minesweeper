using GameEngine.engine.data;
using GameEngine.engine.game_object.components;

namespace GameEngine.engine.game_object; 

public class Transform : Component {
    public delegate void GameObjectInstantiate(GameObject gameObject);
    public static event GameObjectInstantiate? GameObjectInstantiateEvent;
    
    public Transform? Parent { get; private set; }

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

    public Vector2 Up => LocalToWorldMatrix.ConvertVector(Vector2.Up()).Normalized;
    public Vector2 Down => LocalToWorldMatrix.ConvertVector(Vector2.Down()).Normalized;
    public Vector2 Left => LocalToWorldMatrix.ConvertVector(Vector2.Left()).Normalized;
    public Vector2 Right => LocalToWorldMatrix.ConvertVector(Vector2.Right()).Normalized;
    
    public readonly List<Transform> children = new();
    private Vector2 _localPosition;
    private Rotation _localRotation;
    private Vector2 _localScale;
    private TransformationMatrix _localToWorldMatrix = TransformationMatrix.Identity();
    private TransformationMatrix _worldToLocalMatrix = TransformationMatrix.Identity();
    private bool _isDirty = true;
    private bool _isInverseDirty = true;

    public Transform(
        Transform? parent,
        Vector2 position,
        Rotation rotation,
        Vector2 scale,
        bool positionLocal,
        bool rotationLocal,
        bool scaleLocal
    ) {
        parent?.SetChild(this);
        Parent = parent;
        if (positionLocal) {
            LocalPosition = position;
        }
        else {
            Position = position;
        }
        if (rotationLocal) {
            LocalRotation = rotation;
        }
        else {
            Rotation = rotation;
        }
        if (scaleLocal) {
            LocalScale = scale;
        }
        else {
            Scale = scale;
        }
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
    
    public static void Instantiate(GameObject obj) {
        GameObjectInstantiateEvent?.Invoke(obj);
    }

    public GameObject? FindByName(string name) {
        return children.FirstOrDefault(child => child.gameObject.IsActive && child.gameObject.Name == name)?.gameObject;
    }
    
    public GameObject? FindByTag(string tag) {
        return children.FirstOrDefault(child => child.gameObject.IsActive && child.gameObject.Tag == tag)?.gameObject;
    }
    
    public GameObject? FindByNameRecursively(string name) {
        return FindByConditionRecursively(obj => obj.Name == name);
    }

    public GameObject? FindByTagRecursively(string tag) {
        return FindByConditionRecursively(obj => obj.Tag == tag);
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

    private GameObject? FindByConditionRecursively(Predicate<GameObject> condition) {
        foreach (Transform child in children.Where(child => child.gameObject.IsActive)) {
            if (condition.Invoke(child.gameObject)) {
                return child.gameObject;
            }

            GameObject? found = child.FindByConditionRecursively(condition);
            if (found != null) {
                return found;
            }
        }

        return null;
    }
    
    public override string ToString() {
        string parentName = Parent?.gameObject.Name ?? "root";
        return $"ParentName: {parentName}, LocalPosition: {LocalPosition}, WorldPosition: {Position}, LocalRotation: {LocalRotation}, WorldRotation: {Rotation}, LocalScale: {LocalScale}, WorldScale: {Scale}";
    }
}