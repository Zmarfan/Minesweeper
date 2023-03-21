namespace GameEngine.engine.core.cursor; 

public class CursorSettings {
    public readonly bool enabled;
    public readonly bool confine;
    public readonly CustomCursorSettings? customCursorSettings;

    public CursorSettings(bool enabled, bool confine, CustomCursorSettings? customCursorSettings = null) {
        this.enabled = enabled;
        this.confine = confine;
        this.customCursorSettings = customCursorSettings;
    }
}