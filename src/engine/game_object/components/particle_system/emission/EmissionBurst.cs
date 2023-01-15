namespace Worms.engine.game_object.components.particle_system.emission; 

public struct EmissionBurst {
    public readonly float time;
    public readonly RangeZero count;
    public readonly int cycles;
    public readonly float interval;
    public readonly float probability;

    public EmissionBurst(float time, RangeZero count, int cycles, float interval, float probability) {
        this.time = Math.Max(time, 0);
        this.count = count;
        this.cycles = Math.Max(cycles, 0);
        this.interval = Math.Max(interval, 0);
        this.probability = Math.Clamp(probability, 0, 1);
    }
}