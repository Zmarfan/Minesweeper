using Worms.engine.game_object;
using Worms.engine.game_object.components;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.engine.core.game_object_handler; 

public class TrackObject {
    public readonly bool isWorld;
    public bool isActive;
    public readonly List<ToggleComponent> toggleComponents = new();
    public IEnumerable<Collider> Colliders => toggleComponents.OfType<Collider>();
    public IEnumerable<TextureRenderer> TextureRenderers => toggleComponents.OfType<TextureRenderer>();
    public IEnumerable<Script> Scripts => toggleComponents.OfType<Script>();

    public bool MouseInsideTrigger { get; set; } = false;
    
    public TrackObject(bool isWorld, bool isActive) {
        this.isWorld = isWorld;
        this.isActive = isActive;
    }
}