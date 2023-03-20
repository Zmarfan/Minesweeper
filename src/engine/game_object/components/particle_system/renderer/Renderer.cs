using GameEngine.engine.game_object.components.rendering.texture_renderer;

namespace GameEngine.engine.game_object.components.particle_system.renderer; 

public class Renderer {
    public readonly Texture texture;
    public readonly string sortingLayer;
    public readonly int orderInLayer;
    public readonly bool flipX;
    public readonly bool flipY;

    public Renderer(Texture texture, string sortingLayer, int orderInLayer, bool flipX, bool flipY) {
        this.texture = texture;
        this.sortingLayer = sortingLayer;
        this.orderInLayer = orderInLayer;
        this.flipX = flipX;
        this.flipY = flipY;
    }
}