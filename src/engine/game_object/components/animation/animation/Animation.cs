using GameEngine.engine.data;
using GameEngine.engine.game_object.components.animation.composition;

namespace GameEngine.engine.game_object.components.animation.animation; 

public class Animation {
    public delegate void AnimationDelegate();

    public event AnimationDelegate? OnStart;
    public event AnimationDelegate? OnEnd;
    
    private readonly bool _loop;
    private readonly List<Composition> _compositions;
    private readonly int _animationStepLength;

    private readonly ClockTimer _timer;
    private int _lastStep = -1;
    private bool _hasFinished;
    
    public Animation(float stepLengthInSeconds, bool loop, List<Composition> compositions) {
        _loop = loop;
        _compositions = compositions;
        _animationStepLength = compositions.MaxBy(composition => composition.lastStateEndStep)?.lastStateEndStep ?? 0;
        _timer = new ClockTimer(stepLengthInSeconds * _animationStepLength);
    }

    public void Init(GameObject gameObject) {
        foreach (Composition composition in _compositions) {
            composition.Init(gameObject);
        }
    }

    public void Reset() {
        _timer.Reset();
        _hasFinished = false;
    }

    public void Play(float deltaTime) {
        if (_hasFinished) {
            return;
        }
        
        int step = CalculateNextStep();
        _timer.Time += deltaTime;
        if (step == _lastStep) {
            return;
        }
        if (step > _animationStepLength) {
            OnEnd?.Invoke();
            if (_loop) {
                _timer.Reset();
            }
            else {
                _hasFinished = true;
            }
            return;
        }

        RunStep(step);

        _lastStep = step;
    }

    private void RunStep(int step) {
        if (step == 0) {
            OnStart?.Invoke();
        }
        foreach (Composition composition in _compositions) {
            composition.Run(step);
        }
    }
    
    private int CalculateNextStep() {
        return (int)(_timer.Ratio() * _animationStepLength);
    }
}