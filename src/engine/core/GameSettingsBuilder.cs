﻿using Worms.engine.core.audio;
using Worms.engine.core.cursor;
using Worms.engine.core.input.listener;
using Worms.engine.core.renderer.font;
using Worms.engine.core.renderer.textures;
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
    private CursorSettings _cursorSettings = new(true, null);
    private readonly List<FontDefinition> _fontDefinitions = new();
    private readonly List<TextureDeclaration> _textureDeclarations = new();

    private GameSettingsBuilder(AudioSettings audioSettings) {
        _audioSettings = audioSettings;
    }

    public static GameSettingsBuilder Builder(AudioSettings audioSettings) {
        return new GameSettingsBuilder(audioSettings);
    }

    public GameSettings Build() {
        return new GameSettings(_debug, _title, _width, _height, _scenes, _inputListeners, _sortLayers, _audioSettings, _cursorSettings, _fontDefinitions, _textureDeclarations);
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
    
    public GameSettingsBuilder AddInputListeners(IEnumerable<InputListener> listeners) {
        _inputListeners.AddRange(listeners);
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
    
    public GameSettingsBuilder AddFonts(IEnumerable<FontDefinition> fonts) {
        _fontDefinitions.AddRange(fonts);
        return this;
    }
    
    public GameSettingsBuilder AddTextures(IEnumerable<TextureDeclaration> textureDeclarations) {
        _textureDeclarations.AddRange(textureDeclarations);
        return this;
    }
}