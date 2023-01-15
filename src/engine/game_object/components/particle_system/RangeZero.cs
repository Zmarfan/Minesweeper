namespace Worms.engine.game_object.components.particle_system; 

public readonly struct RangeZero {
    public readonly float value1;
    public readonly float value2;

    public RangeZero(float value1, float value2) {
        if (value1 < 0 || value2 < 0) {
            throw new ArgumentException("The values provided can not be lower than zero");
        }
        
        this.value1 = value1;
        this.value2 = value2;
    }

    public RangeZero(float constant) {
        if (constant < 0) {
            throw new ArgumentException("The constant provided can not be lower than zero");
        }
        value1 = constant;
        value2 = constant;
    }
}