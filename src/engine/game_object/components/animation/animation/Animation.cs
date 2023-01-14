using Worms.engine.data;
using Worms.engine.game_object.components.animation.composition;

namespace Worms.engine.game_object.components.animation.animation; 

public class Animation {
    public readonly float stepLengthInSeconds;
    public readonly bool loop;
    private readonly List<Composition> _compositions;
    private readonly int _animationStepLength;

    private readonly ClockTimer _timer;
    private int _lastStep = -1;
    
    public Animation(float stepLengthInSeconds, bool loop, List<Composition> compositions) {
        this.stepLengthInSeconds = stepLengthInSeconds;
        this.loop = loop;
        _compositions = compositions;

        _animationStepLength = compositions.MaxBy(composition => composition.lastStateEndStep)?.lastStateEndStep ?? 0;
        _timer = new ClockTimer(stepLengthInSeconds * _animationStepLength);
    }

    public void Init(GameObject gameObject) {
        foreach (Composition composition in _compositions) {
            composition.Init(gameObject);
        }
    }

    public void Play(float deltaTime) {
        int step = CalculateNextStep();
        _timer.Time += deltaTime;
        if (step == _lastStep) {
            return;
        }
        if (step > _animationStepLength) {
            if (loop) {
                _timer.Reset();
            }
            return;
        }

        RunStep(step);

        _lastStep = step;
    }

    private void RunStep(int step) {
        foreach (Composition composition in _compositions) {
            composition.Run(step);
        }
    }
    
    private int CalculateNextStep() {
        return (int)(_timer.Ratio() * _animationStepLength);
    }
}