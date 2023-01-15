using Worms.engine.data;

namespace Worms.engine.game_object.components.particle_system.particles; 

public class Particles {
    public readonly float duration;
    public readonly bool loop;
    public readonly RangeZero startDelay;
    public readonly RangeZero startLifeTime;
    public readonly Range<float> startSpeed;
    public readonly RangeZero startSize;
    public readonly Range<Rotation> startRotation;
    public readonly float flipRotation;
    public readonly bool playOnAwake;
    public readonly int maxParticles;
    public readonly int seed;
    public readonly StopAction stopAction;

    public Particles(
        float duration, 
        bool loop, 
        RangeZero startDelay, 
        RangeZero startLifeTime, 
        Range<float> startSpeed,
        RangeZero startSize, 
        Range<Rotation> startRotation, 
        float flipRotation, 
        bool playOnAwake, 
        int maxParticles, 
        int seed,
        StopAction stopAction
    ) {
        this.duration = Math.Max(duration, 0);
        this.loop = loop;
        this.startDelay = startDelay;
        this.startLifeTime = startLifeTime;
        this.startSpeed = startSpeed;
        this.startSize = startSize;
        this.startRotation = startRotation;
        this.flipRotation = Math.Clamp(flipRotation, 0, 1);
        this.playOnAwake = playOnAwake;
        this.maxParticles = Math.Max(maxParticles, 0);
        this.seed = seed;
        this.stopAction = stopAction;
    }
}