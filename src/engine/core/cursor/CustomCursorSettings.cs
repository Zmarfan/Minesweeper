namespace GameEngine.engine.core.cursor; 

public class CustomCursorSettings {
    public readonly string imageSource;
    public readonly float xHotSpot;
    public readonly float yHotSpot;

    public CustomCursorSettings(string imageSource, float xHotSpot = 0, float yHotSpot = 0) {
        this.imageSource = imageSource;
        this.xHotSpot = Math.Clamp(xHotSpot, 0, 1);
        this.yHotSpot = Math.Clamp(yHotSpot, 0, 1);
    }
}