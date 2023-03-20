namespace GameEngine.engine.core.cursor; 

public class CursorSettings {
    public readonly bool enabled;
    public readonly CustomCursorSettings? customCursorSettings;

    public CursorSettings(bool enabled, CustomCursorSettings? customCursorSettings = null) {
        this.enabled = enabled;
        this.customCursorSettings = customCursorSettings;
    }
}