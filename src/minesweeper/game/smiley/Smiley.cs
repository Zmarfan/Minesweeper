﻿using GameEngine.engine.core.update.physics.updating;
using GameEngine.engine.data;
using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.engine.game_object.scripts;

namespace Minesweeper.minesweeper.game.smiley; 

public class Smiley : Script {
    public delegate void ClickedDelegate();
    public event ClickedDelegate? Clicked;

    public const float LENGTH = 134;

    private TextureRenderer _textureRenderer = null!;
    private readonly ClockTimer _clickedTimer = new(0.2f);
    private bool _clicked;

    public override void Awake() {
        _textureRenderer = GetComponent<TextureRenderer>();
    }

    public override void Update(float deltaTime) {
        if (_clicked) {
            _clickedTimer.Time += deltaTime;
            if (_clickedTimer.Expired()) {
                ChangeSmiley(SmileyType.DEFAULT);
                _clicked = false;
            }
        }
    }

    public override void OnMouseDown(MouseClickMask mask) {
        Clicked?.Invoke();
    }

    public void WonGame() {
        ChangeSmiley(SmileyType.WON);
    }

    public void Restart() {
        ChangeSmiley(SmileyType.DEFAULT_PRESSED);
        _clicked = true;
        _clickedTimer.Reset();
    }
    
    public void LostGame() {
        ChangeSmiley(SmileyType.LOST);
    }

    private void ChangeSmiley(SmileyType type) {
        _textureRenderer.texture = TextureProvider.GetSmiley(type);
    }
}