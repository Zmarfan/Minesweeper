using Worms.engine.game_object;
using Worms.engine.game_object.components;
using Worms.engine.game_object.components.physics;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering;
using Worms.engine.game_object.scripts;

namespace Worms.engine.core.game_object_handler; 

public class TrackObject {
    public readonly bool isWorld;
    public bool isActive;
    public readonly List<ToggleComponent> toggleComponents;
    public IEnumerable<Collider> Colliders { get; private set; } = null!;
    public IEnumerable<RenderComponent> TextureRenderers { get; private set; } = null!;
    public IEnumerable<Script> Scripts { get; private set; } = null!;

    public bool MouseInsideTrigger { get; set; }
    public HashSet<Collider> CollidersInsideTrigger { get; set; } = new();
    
    public TrackObject(bool isWorld, bool isActive, List<ToggleComponent> toggleComponents) {
        this.isWorld = isWorld;
        this.isActive = isActive;
        this.toggleComponents = toggleComponents;
        SetSpecificComponents();
    }

    public void RemoveComponent(ToggleComponent component) {
        toggleComponents.Remove(component);
        SetSpecificComponents();
    }
    
    private void SetSpecificComponents() {
        Colliders = toggleComponents.OfType<Collider>();
        TextureRenderers = toggleComponents.OfType<RenderComponent>();
        Scripts = toggleComponents.OfType<Script>();
    }
}