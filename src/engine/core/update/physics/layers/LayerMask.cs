namespace Worms.engine.core.update.physics.layers; 

public static class LayerMask {
    public const string DEFAULT = "default";
    public const string IGNORE_RAYCAST = "ignoreRaycast";
    
    private const byte CUSTOM_LAYER_START = 2;
    private const byte MAX_LAYERS = 32;

    private static readonly Dictionary<int, string> NAME_BY_LAYER = new() { { 0, DEFAULT }, { 1, IGNORE_RAYCAST } };
    private static readonly Dictionary<string, int> LAYER_BY_NAME = new() { { DEFAULT, 0 }, { IGNORE_RAYCAST, 1} };
    private static readonly Dictionary<int, int> COLLISION_MASK_BY_LAYER = new();

    public static void Init(Dictionary<string, List<string>> layers) {
        if (layers.Keys.Distinct().Count() < layers.Count || layers.Keys.Count > MAX_LAYERS) {
            throw new Exception("There can be no layers with the same names!");
        }

        List<string> layerNames = layers.Keys.Where(l => l != DEFAULT && l != IGNORE_RAYCAST).ToList();
        for (int i = 0; i < layerNames.Count; i++) {
            int layer = CUSTOM_LAYER_START + i;
            NAME_BY_LAYER.Add(layer, layerNames[i]);
            LAYER_BY_NAME.Add(layerNames[i], layer);
        }

        foreach ((string name, List<string> collisionLayers) in layers) {
            COLLISION_MASK_BY_LAYER.Add(LAYER_BY_NAME[name], CreateMask(collisionLayers.ToArray()));
        }
    }

    public static bool CanLayersInteract(int layer1, int layer2) {
        return (CreateMask(layer1) & COLLISION_MASK_BY_LAYER[layer2]) != 0;
    }
    
    public static int CreateMask(params int[] layers) {
        int mask = 0;
        foreach (int layer in layers) {
            mask |= 1 << layer;
        }

        return mask;
    }
    
    public static int CreateMask(params string[] layerNames) {
        int mask = 0;
        foreach (string layerName in layerNames) {
            mask |= 1 << LAYER_BY_NAME[layerName];
        }

        return mask;
    }
    
    public static int NameToLayer(string layerName) {
        return LAYER_BY_NAME[layerName];
    }
    
    public static string LayerToName(int layer) {
        return NAME_BY_LAYER[layer];
    }
}