using GameEngine.engine.core.update.physics.updating;
using GameEngine.engine.game_object.components;
using GameEngine.engine.game_object.components.physics.colliders;

namespace GameEngine.engine.game_object.scripts; 

public abstract class Script : ToggleComponent {
    public bool HasRunAwake { get; set; }
    public bool HasRunStart { get; set; }
    
    protected Script(bool isActive = true, string name = "script") : base(isActive, name) {
    }

    public virtual void Awake() {
    }

    public virtual void Start() {
    }

    public virtual void FixedUpdate(float deltaTime) {
    }
    
    public virtual void Update(float deltaTime) {
    }

    public virtual void OnTriggerEnter(Collider collider) {
    }

    public virtual void OnTriggerStay(Collider collider) {
    }

    public virtual void OnTriggerExit(Collider collider) {
    }
    
    public virtual void OnMouseEnter() {
    }

    public virtual void OnMouseOver() {
    }

    public virtual void OnMouseExit() {
    }
    
    public virtual void OnMouseDown(MouseClickMask mask) {
    }
}