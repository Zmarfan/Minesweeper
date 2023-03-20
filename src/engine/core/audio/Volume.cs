namespace GameEngine.engine.core.audio; 

public readonly struct Volume {
    private const int MAX_VOLUME = 1;

    public int Percentage => (int)(_fraction * 100);
    private readonly float _fraction = 0f;

    public static Volume Max() {
        return new Volume(MAX_VOLUME);
    }

    public static Volume Zero() {
        return new Volume(0);
    }

    public static Volume FromPercentage(int percentage) {
        return new Volume(Math.Clamp(percentage / 100f, 0f, 1f));
    }
    
    private Volume(float fraction) {
        _fraction = fraction;
    }
    
    public static Volume operator *(Volume v1, Volume v2) {
        return new Volume(v1._fraction * v2._fraction);
    }

    public override string ToString() {
        return $"Vol: {Percentage}%";
    }
}