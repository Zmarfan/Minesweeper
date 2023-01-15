namespace Worms.engine.game_object.components.particle_system; 

public readonly struct Range<T> {
    public readonly T value1;
    public readonly T value2;

    public Range(T value1, T value2) {
        this.value1 = value1;
        this.value2 = value2;
    }

    public Range(T constant) {
        value1 = constant;
        value2 = constant;
    }
}