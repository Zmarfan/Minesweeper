using Worms.engine.data;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class MyTestScript : Script {
    private readonly ClockTimer _timer = new(5f);
    
    public MyTestScript(bool isActive) : base(isActive) {
    }

    public override void Update(float deltaTime) {
        _timer.Time += deltaTime;
        if (_timer.Expired() && TryGetComponent(out TextureRenderer tr)) {
            tr.Destroy();
        }
    }
}