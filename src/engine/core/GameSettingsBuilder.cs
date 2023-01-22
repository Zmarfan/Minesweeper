using Worms.engine.core.audio;
using Worms.engine.core.input.listener;
using Worms.engine.scene;

namespace Worms.engine.core; 

public class GameSettingsBuilder {
    private bool _debug = true;
    private string _title = "My Game";
    private int _width = 600;
    private int _height = 400;
    private readonly List<Scene> _scenes = new();
    private readonly List<InputListener> _inputListeners = new();
    private readonly List<string> _sortLayers = new();
    private readonly AudioSettings _audioSettings;

    private GameSettingsBuilder(AudioSettings audioSettings) {
        _audioSettings = audioSettings;
    }

    public static GameSettingsBuilder Builder(AudioSettings audioSettings) {
        return new GameSettingsBuilder(audioSettings);
    }

    public GameSettings Build() {
        return new GameSettings(_debug, _title, _width, _height, _scenes, _inputListeners, _sortLayers, _audioSettings);
    }

    public GameSettingsBuilder SetDebugMode() {
        _debug = true;
        return this;
    } 
    
    public GameSettingsBuilder SetBuildMode() {
        _debug = false;
        return this;
    } 
        
    public GameSettingsBuilder SetTitle(string title) {
        _title = title;
        return this;
    } 
    
    public GameSettingsBuilder SetWindowWidth(int width) {
        _width = width;
        return this;
    } 
    
    public GameSettingsBuilder SetWindowHeight(int height) {
        _height = height;
        return this;
    }
    
    public GameSettingsBuilder AddScenes(params Scene[] scenes) {
        foreach (Scene scene in scenes) {
            _scenes.Add(scene);
        }
        return this;
    }
    
    public GameSettingsBuilder AddInputListeners(params InputListener[] listeners) {
        foreach (InputListener inputListener in listeners) {
            _inputListeners.Add(inputListener);
        }

        return this;
    }
    
    public GameSettingsBuilder AddSortLayers(params string[] layers) {
        foreach (string layer in layers) {
            _sortLayers.Add(layer);
        }

        return this;
    }
}