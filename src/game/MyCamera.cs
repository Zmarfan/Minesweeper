using Worms.engine.camera;
using Worms.engine.core.cursor;
using Worms.engine.core.input;
using Worms.engine.core.input.listener;
using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class MyCamera : Script {
    public MyCamera() : base(true) {
    }
    
    public override void Awake() {
        Camera.Main.Size = 1;
        Camera.Main.defaultDrawColor = new Color(0.1f, 0.25f, 0f);
    }

    public override void Update(float deltaTime) {
        if (Input.GetButtonDown("cursorToggle")) {
            Cursor.SetActive(!Cursor.IsActive);
        }

        if (!Cursor.IsActive) {
            Camera.Main.Position += Input.MouseDirection * 200f * Camera.Main.Size * deltaTime;
        }
        Camera.Main.Size += Input.GetAxis("cameraZoom").x * deltaTime;
    }
}