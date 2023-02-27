using Worms.engine.core.gizmos;
using Worms.engine.core.update.physics;
using Worms.engine.core.update.physics.updating;
using Worms.engine.data;

namespace Worms.engine.game_object.components.physics.colliders; 

public class CircleCollider : Collider {
    private const int CIRCLE_TO_POLYGON_POINT_COUNT = 15;
    
    public float radius;
    
    public Vector2[] CircleAsPoints {
        get {
            float theta = (float)(Math.PI * 2 / _circlePoints.Length);
            for (int i = 0; i < _circlePoints.Length; i++) {
                Rotation angle = Rotation.FromRadians(theta * i);
                Vector2 point = offset + new Vector2((float)(radius * Math.Cos(angle.Radians)), (float)(radius * Math.Sin(angle.Radians)));
                _circlePoints[i] = Transform.LocalToWorldMatrix.ConvertPoint(point);
            }

            return _circlePoints;
        }
    }
    
    private readonly Vector2[] _circlePoints = new Vector2[CIRCLE_TO_POLYGON_POINT_COUNT];
    
    public CircleCollider(
        bool isActive, 
        ColliderState state,
        float radius,
        Vector2 offset
    ) : base(isActive, state, offset) {
        this.radius = radius;
    }

    public override Vector2[] GetLocalCorners() {
        localCorners[0] = offset + new Vector2(-radius, -radius);
        localCorners[1] = offset + new Vector2(-radius, radius);
        localCorners[2] = offset + new Vector2(radius, radius);
        localCorners[3] = offset + new Vector2(radius, -radius);
        return localCorners;
    }

    public override bool IsPointInside(Vector2 p) {
        return (Transform.WorldToLocalMatrix.ConvertPoint(p) - offset).SqrMagnitude <= radius * radius;
    }

    public override ColliderHit? Raycast(Vector2 origin, Vector2 direction) {
        origin = Transform.WorldToLocalMatrix.ConvertPoint(origin);
        direction = Transform.WorldToLocalMatrix.ConvertVector(direction);
        if (PhysicsUtils.LineCircleIntersection(origin, direction, offset, radius, out Vector2 intersection)) {
            return new ColliderHit(
                Transform.LocalToWorldMatrix.ConvertPoint(intersection),
                Transform.LocalToWorldMatrix.ConvertVector(intersection - offset).Normalized
            );
        }

        return null;
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawEllipsis(Center, radius * Transform.Scale, Transform.Rotation, GizmoId);
        Gizmos.DrawPolygon(CircleAsPoints,GizmoSettings.CIRCLE_POLYGON_NAME);
        base.OnDrawGizmos();
    }
}