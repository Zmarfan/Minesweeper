using Worms.engine.data;

namespace Worms.engine.game_object.components.particle_system.emission; 

public class EmissionBurst {
    private readonly int _initCycles;
    private readonly RangeZero _count;
    private readonly float _probability;

    private int _cyclesLeftToRun;
    private readonly ClockTimer _startTimer;
    private readonly ClockTimer _intervalTimer;
    
    public EmissionBurst(float time, RangeZero count, int cycles, float interval, float probability) {
        _initCycles = cycles;
        _probability = Math.Clamp(probability, 0, 1);
        _count = count;

        _cyclesLeftToRun = Math.Max(cycles, 0);
        _startTimer = new ClockTimer(Math.Max(time, 0));
        _intervalTimer = new ClockTimer(Math.Max(interval, 0));
        
        Reset();
    }

    public void Reset() {
        _cyclesLeftToRun = _initCycles;
        _startTimer.Reset();
        _intervalTimer.Reset();
        _intervalTimer.Time = _intervalTimer.Duration;
    }
    
    public int GetAmountOfParticlesToBurst(float deltaTime, Random random) {
        if (_cyclesLeftToRun == 0) {
            return 0;
        }
        
        _startTimer.Time += deltaTime;
        if (_startTimer.Expired()) {
            _intervalTimer.Time += deltaTime;
            if (_intervalTimer.Expired()) {
                _cyclesLeftToRun--;
                _intervalTimer.Reset();
                return CalculateBurstAmount(random);
            }
        }

        return 0;
    }

    private int CalculateBurstAmount(Random random) {
        if (random.NextDouble() > _probability) {
            return 0;
        }

        return (int)_count.GetRandom(random);
    }
}