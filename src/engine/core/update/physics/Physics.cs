using Worms.engine.core.game_object_handler;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.scene;

namespace Worms.engine.core.update.physics; 

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

    public static bool Raycast(Vector2 origin, Vector2 direction, out RaycastHit? hit) {
        return Raycast(origin, direction, 5000f, out hit);
    }

    public static bool Raycast(Vector2 origin, Vector2 direction, float maxDistance, out RaycastHit? hit) {
        direction = direction.Normalized * maxDistance;
        List<RaycastHit> hits = new();
        foreach ((GameObject _, TrackObject obj) in _self.GameObjectHandler.objects) {
            if (!obj.isActive) {
                continue;
            }

            hits.AddRange(CalculateRaycastHitsOnColliders(origin, direction, obj.Colliders));
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
            .Where(c => c.IsActive && c.state != ColliderState.TRIGGER)
            .SelectMany(c => CalculateRaycastHit(origin, direction, c));
    }

    private static IEnumerable<RaycastHit> CalculateRaycastHit(Vector2 origin, Vector2 direction, Collider collider) {
        ColliderHit? hit = collider.Raycast(origin, direction);
        if (hit == null) {
            return new List<RaycastHit>();
        }

        return new List<RaycastHit> { new(collider, (hit.point - origin).Magnitude, hit.normal, hit.point, collider.Transform) };
    }
}