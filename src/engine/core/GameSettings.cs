using Worms.engine.core.audio;
using Worms.engine.core.cursor;
using Worms.engine.core.input.listener;
using Worms.engine.core.renderer.font;
using Worms.engine.scene;

namespace Worms.engine.core; 

public record GameSettings(
    bool debug,
    string title,
    int width, 
    int height,
    List<Scene> scenes, 
    List<InputListener> inputListeners,
    List<string> sortLayers,
    AudioSettings audioSettings,
    CursorSettings cursorSettings,
    IEnumerable<FontDefinition> fontDefinitions
) {
    public readonly bool debug = debug;
    public readonly string title = title;
    public int width = width;
    public int height = height;
    public readonly List<Scene> scenes = scenes;
    public readonly List<InputListener> inputListeners = inputListeners;
    public readonly List<string> sortLayers = sortLayers;
    public readonly AudioSettings audioSettings = audioSettings;
    public readonly CursorSettings cursorSettings = cursorSettings;
    public readonly IEnumerable<FontDefinition> fontDefinitions = fontDefinitions;
}