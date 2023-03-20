using GameEngine.engine.data;

namespace GameEngine.engine.game_object.components.rendering; 

public class RenderComponent : ToggleComponent {
    public string sortingLayer;
    public int orderInLayer;
    public virtual Color Color { 
        get => color;
        set => color = value;
    }
    protected Color color;

    protected RenderComponent(string sortingLayer, int orderInLayer, Color color, bool isActive = true, string name = "renderComponent") : base(isActive, name) {
        this.sortingLayer = sortingLayer;
        this.orderInLayer = orderInLayer;
        this.color = color;
    }
}