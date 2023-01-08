using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class MyTestScript : Script {
    private float _timer = 0;
    
    public MyTestScript(bool isActive) : base(isActive) {
    }

    public override void Update(float deltaTime) {
        _timer += deltaTime;
        if (_timer > 5) {
            GetComponent<TextureRenderer>().IsActive = false;
        }
    }
}