using Worms.engine.core.assets;
using Worms.engine.core.audio;
using Worms.engine.core.cursor;
using Worms.engine.core.input.listener;
using Worms.engine.core.renderer.textures;
using Worms.engine.core.update.physics.layers;
using Worms.engine.core.update.physics.settings;
using Worms.engine.helper;
using Worms.engine.scene;

namespace Worms.engine.core; 

public class GameSettingsBuilder {
    private bool _debug = true;
    private string _title = "My Game";
    private int _width = 600;
    private int _height = 400;
    private Assets _assets = new(ListUtils.Empty<AssetDeclaration>(), ListUtils.Empty<AssetDeclaration>(), ListUtils.Empty<AssetDeclaration>());
    private readonly List<Scene> _scenes = new();
    private readonly List<InputListener> _inputListeners = new();
    private PhysicsSettings _physicsSettings = PhysicsSettingsBuilder
        .Builder(
            ListUtils.Of(LayerMask.DEFAULT, LayerMask.IGNORE_RAYCAST),
            ListUtils.Of(LayerMask.DEFAULT, LayerMask.IGNORE_RAYCAST)
        )
        .Build();
    private readonly List<string> _sortLayers = new();
    private AudioSettings _audioSettings = new(Volume.Max(), ListUtils.Empty<AudioChannel>());
    private CursorSettings _cursorSettings = new(true, null);
    
    public static GameSettingsBuilder Builder() {
        return new GameSettingsBuilder();
    }

    public GameSettings Build() {
        return new GameSettings(_debug, _title, _width, _height, _assets, _scenes, _inputListeners, _physicsSettings, _sortLayers, _audioSettings, _cursorSettings);
    }

    public GameSettingsBuilder SetAssets(Assets assets) {
        _assets = assets;
        return this;
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
    
    public GameSettingsBuilder AddScenes(IEnumerable<Scene> scenes) {
        _scenes.AddRange(scenes);
        return this;
    }
    
    public GameSettingsBuilder SetAudioSettings(AudioSettings audioSettings) {
        _audioSettings = audioSettings;
        return this;
    }
    
    public GameSettingsBuilder AddInputListeners(IEnumerable<InputListener> listeners) {
        _inputListeners.AddRange(listeners);
        return this;
    }
    
    public GameSettingsBuilder SetPhysics(PhysicsSettings settings) {
        _physicsSettings = settings;
        return this;
    }
    
    public GameSettingsBuilder AddSortLayers(IEnumerable<string> layers) {
        _sortLayers.AddRange(layers);
        return this;
    }

    public GameSettingsBuilder SetCursorSettings(CursorSettings settings) {
        _cursorSettings = settings;
        return this;
    }
}