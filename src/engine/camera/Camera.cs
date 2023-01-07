using Worms.engine.data;

namespace Worms.engine.camera; 

public class Camera {
    public Vector2 Position { get; set; } = Vector2.Zero();
    public Rotation Rotation { get; set; } = Rotation.Normal();
    public float Size { get; set; }

    public Camera(float size) {
        Size = size;
    }
}