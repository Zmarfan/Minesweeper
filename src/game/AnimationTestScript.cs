using Worms.engine.core.input;
using Worms.engine.game_object.components.animation.animation;
using Worms.engine.game_object.components.animation.controller;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class AnimationTestScript : Script {
    private AnimationController _animationController = null!;
    
    public AnimationTestScript() : base(true) {
    }

    public override void Awake() {
        _animationController = GetComponent<AnimationController>();
    }

    public override void Update(float deltaTime) {
        if (Input.GetButtonDown("animationTest1")) {
            _animationController.SetTrigger("trigger1");
        }
        if (Input.GetButtonDown("animationTest2")) {
            _animationController.SetTrigger("trigger2");
        }
        if (Input.GetButtonDown("animationTest3")) {
            _animationController.SetTrigger("trigger3");
        }
    }
}