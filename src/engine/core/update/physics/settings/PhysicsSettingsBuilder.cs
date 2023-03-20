using GameEngine.engine.core.update.physics.layers;

namespace GameEngine.engine.core.update.physics.settings; 

public class PhysicsSettingsBuilder {
    private readonly Dictionary<string, List<string>> _layersToCollisionLayers = new() {
        { LayerMask.DEFAULT, new List<string>() },
        { LayerMask.IGNORE_RAYCAST, new List<string>() }
    };

    private PhysicsSettingsBuilder(IEnumerable<string> defaultCollisionLayers, IEnumerable<string> ignoreRaycastCollisionLayers) {
        _layersToCollisionLayers[LayerMask.DEFAULT].AddRange(defaultCollisionLayers);
        _layersToCollisionLayers[LayerMask.IGNORE_RAYCAST].AddRange(ignoreRaycastCollisionLayers);
    }

    public static PhysicsSettingsBuilder Builder(
        IEnumerable<string> defaultCollisionLayers,
        IEnumerable<string> ignoreRaycastCollisionLayers
    ) {
        return new PhysicsSettingsBuilder(defaultCollisionLayers, ignoreRaycastCollisionLayers);
    }

    public PhysicsSettings Build() {
        SetCollisionLayersToBeSymmetric();
        return new PhysicsSettings(_layersToCollisionLayers);
    }

    public PhysicsSettingsBuilder AddLayer(string layer, List<string> collisionLayers) {
        _layersToCollisionLayers.Add(layer, collisionLayers);
        return this;
    }
    
    private void SetCollisionLayersToBeSymmetric() {
        foreach ((string layer, List<string> collisionLayers) in _layersToCollisionLayers) {
            List<string> content = new(collisionLayers);
            foreach (string collisionLayer in content) {
                if (!_layersToCollisionLayers[collisionLayer].Contains(layer)) {
                    _layersToCollisionLayers[collisionLayer].Add(layer);
                }
            }
        }
    }
}