using Worms.engine.camera;
using Worms.engine.core.cursor;
using Worms.engine.core.input;
using Worms.engine.core.input.listener;
using Worms.engine.data;

namespace Worms.game; 

public class MyCamera : Camera {
    public override void Awake() {
        Size = 1;
        defaultDrawColor = new Color(0.1f, 0.25f, 0f);
    }

    public override void Update(float deltaTime) {
        if (Input.GetButtonDown("cursorToggle")) {
            Cursor.SetActive(!Cursor.IsActive);
        }

        if (!Cursor.IsActive) {
            Position += Input.MouseDirection * 500f * Size * deltaTime;
        }
        Size += Input.GetAxis("cameraZoom").x * deltaTime;
    }
}