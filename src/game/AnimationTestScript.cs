using Worms.engine.game_object.components.animation.animation;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class AnimationTestScript : Script {
    private readonly Animation _animation;
    
    public AnimationTestScript() : base(true) {
        _animation = AnimationFactory.CreateTextureAnimation(0.05f, true, 19);
    }

    public override void Awake() {
        _animation.Init(gameObject);
    }

    public override void Update(float deltaTime) {
        _animation.Play(deltaTime);
    }
}