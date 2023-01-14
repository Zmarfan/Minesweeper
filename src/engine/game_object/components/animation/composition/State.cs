namespace Worms.engine.game_object.components.animation.composition; 

public struct State {
    public readonly Action<Component> action;
    public readonly int timeUnitWaitAfter;

    public State(Action<Component> action, int timeUnitWaitAfter) {
        this.action = action;
        this.timeUnitWaitAfter = timeUnitWaitAfter;
    }
}