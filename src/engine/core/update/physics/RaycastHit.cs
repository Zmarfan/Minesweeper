using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components.physics.colliders;

namespace GameEngine.engine.core.update.physics; 

public struct RaycastHit {
    public readonly Collider collider;
    public readonly float distance;
    public readonly Vector2 normal;
    public readonly Vector2 point;
    public readonly Transform transform;

    public RaycastHit(Collider collider, float distance, Vector2 normal, Vector2 point, Transform transform) {
        this.collider = collider;
        this.distance = distance;
        this.normal = normal;
        this.point = point;
        this.transform = transform;
    }
}