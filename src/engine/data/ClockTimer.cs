namespace GameEngine.engine.data; 

public class ClockTimer {
    private float _time;

    public float Time {
        get => _time;
        set {
            if (value >= _time)
                _time = value;
            else
                throw new Exception("Time can only count up!");
        }
    }

    public float Duration { get; set; }

    public ClockTimer(float duration, float time = 0) {
        if (duration < 0) {
            throw new Exception("Duration can not be less than 0");
        }
        Duration = duration;
        if (time < 0) {
            throw new Exception("Time can not be less than 0");
        }

        _time = time;
    }

    public ClockTimer(ClockTimer clockTimer) {
        _time = clockTimer._time;
        Duration = clockTimer.Duration;
    }

    public bool Expired() {
        return _time > Duration;
    }

    public void Reset() {
        _time = 0;
    }

    public float Ratio() {
        return _time / Duration;
    }

    public float InverseRatio() {
        return 1.0f - _time / Duration;
    }
}