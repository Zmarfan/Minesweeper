using Worms.engine.core.assets;
using Worms.engine.core.audio;
using Worms.engine.core.cursor;
using Worms.engine.core.input.listener;
using Worms.engine.core.update.physics.settings;
using Worms.engine.scene;

namespace Worms.engine.core; 

public record GameSettings(
    bool debug,
    string title,
    int width, 
    int height,
    Assets assets,
    List<Scene> scenes, 
    List<InputListener> inputListeners,
    PhysicsSettings physicsSettings,
    List<string> sortLayers,
    AudioSettings audioSettings,
    CursorSettings cursorSettings
) {
    public readonly bool debug = debug;
    public readonly string title = title;
    public int width = width;
    public int height = height;
    public readonly Assets assets = assets;
    public readonly List<Scene> scenes = scenes;
    public readonly List<InputListener> inputListeners = inputListeners;
    public readonly PhysicsSettings physicsSettings = physicsSettings;
    public readonly List<string> sortLayers = sortLayers;
    public readonly AudioSettings audioSettings = audioSettings;
    public readonly CursorSettings cursorSettings = cursorSettings;
}