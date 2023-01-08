using Worms.engine.camera;
using Worms.engine.data;

namespace Worms.game; 

public class MyCamera : Camera {
    public override void Awake() {
        Size = 7;
        defaultDrawColor = new Color(0.1f, 0.25f, 0f);
    }

    public override void Update(float deltaTime) {
        // Position += Vector2.Right() * deltaTime * 0.1f;
    }
}