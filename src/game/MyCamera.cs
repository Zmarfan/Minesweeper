using Worms.engine.camera;
using Worms.engine.core.input;
using Worms.engine.data;

namespace Worms.game; 

public class MyCamera : Camera {
    public override void Awake() {
        Size = 1;
        defaultDrawColor = new Color(0.1f, 0.25f, 0f);
    }

    public override void Update(float deltaTime) {
        Position += Input.MouseDirection * 5f * Size;
        Size += Input.GetAxis("cameraZoom").x * deltaTime;
    }
}