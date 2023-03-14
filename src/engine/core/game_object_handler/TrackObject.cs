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
    public readonly List<ToggleComponent> toggleComponents = new();
    public Collider? Collider => toggleComponents.OfType<Collider>().FirstOrDefault();
    public IEnumerable<RenderComponent> TextureRenderers => toggleComponents.OfType<RenderComponent>();
    public IEnumerable<Script> Scripts => toggleComponents.OfType<Script>();

    public bool MouseInsideTrigger { get; set; }
    public HashSet<Collider> CollidersInsideTrigger { get; set; } = new();
    
    public TrackObject(bool isWorld, bool isActive) {
        this.isWorld = isWorld;
        this.isActive = isActive;
    }
}