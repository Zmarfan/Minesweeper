using Worms.engine.data;

namespace Worms.engine.game_object.components.rendering; 

public class RenderComponent : ToggleComponent {
    public string sortingLayer;
    public int orderInLayer;
    public virtual Color Color { 
        get => color;
        set => color = value;
    }
    protected Color color;

    protected RenderComponent(bool isActive, string sortingLayer, int orderInLayer, Color color) : base(isActive) {
        this.sortingLayer = sortingLayer;
        this.orderInLayer = orderInLayer;
        this.color = color;
    }
}