using Worms.engine.game_object.components.animation.animation;

namespace Worms.engine.game_object.components.animation.controller; 

public class AnimationControllerBuilder {
    private bool _isActive = true;
    private readonly List<Tuple<string, Animation>> _animationsWithTriggers = new();
    private Animation? _startAnimation = null;

    public AnimationController Build() {
        return new AnimationController(_startAnimation, _animationsWithTriggers, _isActive);
    }
        
    public static AnimationControllerBuilder Builder() {
        return new AnimationControllerBuilder();
    }

    public AnimationControllerBuilder AddAnimation(string trigger, Animation animation) {
        _animationsWithTriggers.Add(new Tuple<string, Animation>(trigger, animation));
        return this;
    }

    public AnimationControllerBuilder SetIsActive(bool isActive) {
        _isActive = isActive;
        return this;
    }

    public AnimationControllerBuilder SetStartAnimation(int index) {
        _startAnimation = _animationsWithTriggers[index].Item2;
        return this;
    }
}