namespace GameEngine.engine.core.update.physics.settings; 

public record PhysicsSettings(Dictionary<string, List<string>> layersToCollisionLayers) {
    public readonly Dictionary<string, List<string>> layersToCollisionLayers = layersToCollisionLayers;
}