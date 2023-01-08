using Worms.engine.data;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class Transform : Component {
    public Transform? Parent { get; }
    public Vector2 LocalPosition { get; set; } = Vector2.Zero();
    public Rotation LocalRotation { get; set; } = Rotation.Normal();
    public float LocalScale { get; set; } = 1;
    public readonly List<Transform> children = new();

    public Vector2 WorldPosition {
        get {
            if (Parent == null) {
                return LocalPosition;
            }

            return Parent.WorldPosition + Vector2.RotatePointAroundPoint(LocalPosition * Parent.WorldScale, Vector2.Zero(), -Parent.WorldRotation.Value);
        }
    }
    
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

    public override string ToString() {
        string parentName = Parent?.gameObject.Name ?? "root";
        return $"ParentName: {parentName}, LocalPosition: {LocalPosition}, WorldPosition: {WorldPosition}, LocalRotation: {LocalRotation}, WorldRotation: {WorldRotation}, LocalScale: {LocalScale}, WorldScale: {WorldScale}";
    }
}