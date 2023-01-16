using Worms.engine.game_object.components.particle_system.ranges;

namespace Worms.engine.game_object.components.particle_system.emission; 

public class EmissionBuilder {
    private RangeZero _rateOverTime = new(10);
    private readonly List<EmissionBurst> _bursts = new();

    public static EmissionBuilder Builder() {
        return new EmissionBuilder();
    }

    public Emission Build() {
        return new Emission(_rateOverTime, _bursts);
    }

    public EmissionBuilder SetRateOverTime(RangeZero range) {
        _rateOverTime = range;
        return this;
    }

    public EmissionBuilder AddBurst(EmissionBurst burst) {
        _bursts.Add(burst);
        return this;
    }
}