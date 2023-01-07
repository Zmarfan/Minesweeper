using Worms.engine.data;

namespace Worms.engine.camera; 

public class Camera {
    public Vector2 LocalPosition { get; set; } = Vector2.Zero();
    public Rotation LocalRotation { get; set; } = Rotation.Normal();
    public float Size { get; set; } = 1;
}