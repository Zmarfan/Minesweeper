namespace Worms.engine.core.update.physics.layers; 

public class LayerMask {
    public const string DEFAULT = "default";
    public const string IGNORE_RAYCAST = "ignoreRaycast";
    
    private const byte MAX_CUSTOM_LAYERS = 30;
    private const byte START_CUSTOM_LAYER = 2;

    private static readonly Dictionary<int, string> NAME_BY_LAYER = new() { { 0, DEFAULT }, { 1, IGNORE_RAYCAST } };
    private static readonly Dictionary<string, int> LAYER_BY_NAME = new() { { DEFAULT, 0 }, { IGNORE_RAYCAST, 1 } };

    public static void Init(IReadOnlyList<string> layers) {
        if (layers.Distinct().Count() < layers.Count || layers.Count > MAX_CUSTOM_LAYERS) {
            throw new Exception("There can be no layers with the same names!");
        }

        for (byte i = 0; i < layers.Count; i++) {
            int id = START_CUSTOM_LAYER + i;
            NAME_BY_LAYER.Add(id, layers[i]);
            LAYER_BY_NAME.Add(layers[i], id);
        }
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