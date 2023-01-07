﻿using System.Text;
using Worms.engine.data;
using Worms.engine.game_object.components;

namespace Worms.engine.game_object; 

public class Transform : Component {
    public Transform? Parent { get; }
    public Vector2 LocalPosition { get; set; } = Vector2.Zero();
    public Rotation LocalRotation { get; set; } = Rotation.Normal();
    public float LocalScale { get; set; } = 1;
    private readonly List<Transform> _children = new();

    public Vector2 WorldPosition {
        get {
            if (Parent == null) {
                return LocalPosition;
            }

            return Parent.WorldPosition + LocalPosition;
        }
        set => LocalPosition = value - Parent?.WorldPosition ?? Vector2.Zero();
    }
    
    public Rotation WorldRotation {
        get {
            if (Parent == null) {
                return LocalRotation;
            }

            return Parent.WorldRotation + LocalRotation;
        }
        set => LocalRotation = value - Parent?.WorldRotation ?? Rotation.Normal();
    }
    
    public float WorldScale {
        get {
            if (Parent == null) {
                return LocalScale;
            }

            return Parent.WorldScale * LocalScale;
        }
        set => LocalScale = value * Parent?.WorldScale ?? 1;
    }
    
    public Transform(Transform? parent, Vector2 localPosition, Rotation localRotation, float localScale) {
        Parent = parent;
        LocalPosition = localPosition;
        LocalRotation = localRotation;
        LocalScale = localScale;
        parent?.SetChild(this);
    }

    private void SetChild(Transform child) {
        if (_children.Contains(child)) {
            throw new ArgumentException("Can not add child to parent who already has that child");
        }
        _children.Add(child);
    }

    public override string ToString() {
        string parentName = Parent?.GameObject.Name ?? "root";
        string children = _children.Count == 0 ? string.Empty : _children
            .Select(child => $"{child.GameObject}")
            .Aggregate(new StringBuilder("\nChildren:\n"), (acc, entry) => acc.Append(entry))
            .ToString();
        return $"ParentName: {parentName}, LocalPosition: {LocalPosition}, WorldPosition: {WorldPosition}, LocalRotation: {LocalRotation}, WorldRotation: {WorldRotation}, LocalScale: {LocalScale}, WorldScale: {WorldScale}{children}";
    }
}