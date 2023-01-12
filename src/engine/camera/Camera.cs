using Worms.engine.data;

namespace Worms.engine.camera; 

public abstract class Camera {
    public Vector2 Position { get; set; } = Vector2.Zero();
    public float Size { get; set; } = 1;
    public Color defaultDrawColor = Color.Black();

    public virtual void Awake() {
    }

    public virtual void Update(float deltaTime) {
    }
}