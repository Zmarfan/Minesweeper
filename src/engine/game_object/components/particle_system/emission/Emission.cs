using Worms.engine.game_object.components.particle_system.ranges;

namespace Worms.engine.game_object.components.particle_system.emission; 

public class Emission {
    public readonly RangeZero rateOverTime;
    private readonly List<EmissionBurst> _bursts;

    public Emission(RangeZero rateOverTime, List<EmissionBurst> bursts) {
        this.rateOverTime = rateOverTime;
        _bursts = bursts;
    }

    public int CalculateBurstAmount(float deltaTime, Random random) {
        return _bursts.Sum(burst => burst.GetAmountOfParticlesToBurst(deltaTime, random));
    }

    public void Reset() {
        _bursts.ForEach(burst => burst.Reset());
    }

    public Emission Clone() {
        return new Emission(
            rateOverTime,
            _bursts.Select(burst => burst.Clone()).ToList()
        );
    }
}