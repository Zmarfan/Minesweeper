namespace Worms.engine.game_object.components.particle_system.emission; 

public class Emission {
    public readonly RangeZero rateOverTime;
    public readonly IEnumerable<EmissionBurst> bursts;

    public Emission(RangeZero rateOverTime, IEnumerable<EmissionBurst> bursts) {
        this.rateOverTime = rateOverTime;
        this.bursts = bursts;
    }
}