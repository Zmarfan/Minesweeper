using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.engine.core.game_object_handler; 

public class TrackObject {
    public readonly bool isWorld;
    public bool isActive;
    public readonly List<TextureRenderer> textureRenderers = new();
    public readonly List<Script> scripts = new();

    public TrackObject(bool isWorld, bool isActive) {
        this.isWorld = isWorld;
        this.isActive = isActive;
    }
}