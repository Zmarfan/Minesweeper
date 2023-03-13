using Worms.engine.camera;
using Worms.engine.game_object.scripts;

namespace Worms.game.asteroids.camera; 

public class CameraInit : Script {
    public CameraInit() : base(true) {
    }

    public override void Awake() {
        Camera.Main.Size = 2;
    }
}