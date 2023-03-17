using Worms.engine.core.game_object_handler;
using Worms.engine.core.update.physics.layers;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.scene;

namespace Worms.engine.core.update.physics; 

public class Physics {
    private static readonly int DEFAULT_LAYER_MASK = ~LayerMask.CreateMask(LayerMask.IGNORE_RAYCAST);
    
    private static Physics _self = null!;

    private readonly SceneData _sceneData;
    private GameObjectHandler GameObjectHandler => _sceneData.gameObjectHandler;
    
    private Physics(SceneData sceneData) {
        _sceneData = sceneData;
    }

    public static void Init(SceneData sceneData) {
        if (_self != null) {
            throw new Exception("There can only be one Physics instance as it's a singleton!");
        }

        _self = new Physics(sceneData);
    }
    
    public static bool LineCast(Vector2 from, Vector2 to, out RaycastHit? hit) {
        return LineCast(from, to, DEFAULT_LAYER_MASK, out hit);
    }

    public static bool Raycast(Vector2 origin, Vector2 direction, out RaycastHit? hit) {
        return Raycast(origin, direction, DEFAULT_LAYER_MASK, out hit);
    }
    
    public static bool LineCast(Vector2 from, Vector2 to, int layerMask, out RaycastHit? hit) {
        Vector2 direction = to - from;
        return Raycast(from, direction, direction.Magnitude, layerMask, out hit);
    }

    public static bool Raycast(Vector2 origin, Vector2 direction, int layerMask, out RaycastHit? hit) {
        return Raycast(origin, direction, 5000f, layerMask, out hit);
    }

    public static bool Raycast(Vector2 origin, Vector2 direction, float maxDistance, int layerMask, out RaycastHit? hit) {
        direction = direction.Normalized * maxDistance;
        hit = null;
        foreach ((GameObject gameObject, TrackObject obj) in _self.GameObjectHandler.objects) {
            if (!obj.isActive || (LayerMask.CreateMask(gameObject.Layer) & layerMask) == 0) {
                continue;
            }

            hit = CalculateBestRaycastHitOnCollider(origin, direction, obj.Colliders, hit);
        }

        return hit.HasValue;
    }

    private static RaycastHit? CalculateBestRaycastHitOnCollider(
        Vector2 origin,
        Vector2 direction,
        IEnumerable<Collider> colliders,
        RaycastHit? bestHit
    ) {
        foreach (Collider collider in colliders) {
            if (collider is not { IsActive: true } || collider.state == ColliderState.TRIGGER) {
                continue;
            }

            RaycastHit? hit = CalculateRaycastHit(origin, direction, collider);
            if (hit != null && (bestHit == null || bestHit.Value.distance > hit.Value.distance)) {
                bestHit = hit.Value;
            }
        }

        return bestHit;
    }

    private static RaycastHit? CalculateRaycastHit(Vector2 origin, Vector2 direction, Collider collider) {
        ColliderHit? hit = collider.Raycast(origin, direction);
        if (hit == null) {
            return null;
        }

        return new RaycastHit(collider, (hit.point - origin).Magnitude, hit.normal, hit.point, collider.Transform);
    }
}