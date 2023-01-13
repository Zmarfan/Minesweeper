using Worms.engine.camera;
using Worms.engine.core.audio;
using Worms.engine.core.input.listener;
using Worms.engine.game_object;

namespace Worms.engine.core; 

public record GameSettings(bool debug, string title, int width, int height, Camera camera, GameObject root, List<InputListener> inputListeners, List<string> sortLayers, AudioSettings audioSettings) {
    public readonly bool debug = debug;
    public readonly string title = title;
    public int width = width;
    public int height = height;
    public readonly Camera camera = camera;
    public readonly GameObject root = root;
    public readonly List<InputListener> inputListeners = inputListeners;
    public readonly List<string> sortLayers = sortLayers;
    public readonly AudioSettings audioSettings = audioSettings;
}