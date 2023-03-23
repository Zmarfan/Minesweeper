namespace GameEngine.engine.core.update.physics.updating; 

public readonly struct MouseClickMask {
    public const int LEFT = 0b0000001;
    public const int RIGHT = 0b0000010;
    public const int MIDDLE = 0b0000100;

    public bool LeftClick => (mask & LEFT) == LEFT;
    public bool RightClick => (mask & RIGHT) == RIGHT;
    public bool MiddleClick => (mask & MIDDLE) == MIDDLE;
    public readonly int mask;

    public MouseClickMask(int mask) {
        this.mask = mask;
    }
}