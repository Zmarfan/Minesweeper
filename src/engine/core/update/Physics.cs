using Worms.engine.core.game_object_handler;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.colliders;
using Worms.engine.scene;

namespace Worms.engine.core.update; 

public class Physics {
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

    public static bool Raycast(Vector2 origin, Vector2 direction, float maxDistance, out RaycastHit? hit) {
        List<RaycastHit> hits = new();
        foreach ((GameObject _, TrackObject obj) in _self.GameObjectHandler.objects) {
            if (!obj.isActive) {
                continue;
            }

            hits.AddRange(
                CalculateRaycastHitsOnColliders(origin, direction, obj.Colliders)
                    .Where(h => h.distance <= maxDistance)
            );
        }

        if (hits.Count == 0) {
            hit = null;
            return false;
        }

        hit = hits.MinBy(h => h.distance);
        return true;
    }

    private static IEnumerable<RaycastHit> CalculateRaycastHitsOnColliders(Vector2 origin, Vector2 direction, IEnumerable<Collider> colliders) {
        return colliders
            .Where(c => c is { IsActive: true, isTrigger: false })
            .Select(c => CalculateRaycastHit(origin, direction, c))
            .Where(c => c.HasValue)
            .Select(c => c!.Value);
    }

    private static RaycastHit? CalculateRaycastHit(Vector2 origin, Vector2 direction, Collider collider) {
        ColliderHit? hit = collider.Raycast(origin, direction);
        if (hit == null) {
            return null;
        }

        return new RaycastHit(collider, (hit.point - origin).Magnitude, hit.normal, hit.point, collider.Transform);
    }
}