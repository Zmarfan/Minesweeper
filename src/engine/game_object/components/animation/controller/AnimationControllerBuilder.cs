using GameEngine.engine.game_object.components.animation.animation;

namespace GameEngine.engine.game_object.components.animation.controller; 

public class AnimationControllerBuilder {
    private bool _isActive = true;
    private string _name = "animationController";
    private readonly List<Tuple<string, Animation>> _animationsWithTriggers = new();
    private Animation? _startAnimation;

    public AnimationController Build() {
        return new AnimationController(_startAnimation, _animationsWithTriggers, _isActive, _name);
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

    public AnimationControllerBuilder SetName(string name) {
        _name = name;
        return this;
    }
    
    public AnimationControllerBuilder SetStartAnimation(int index) {
        _startAnimation = _animationsWithTriggers[index].Item2;
        return this;
    }
}