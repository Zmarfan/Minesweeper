﻿using Worms.engine.core.input;
using Worms.engine.core.input.listener;
using Worms.engine.data;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.physics.colliders; 

public class TextureCollider : Script {
    public Texture Texture {
        get => _textureRenderer.texture;
        set {
            _textureRenderer.texture = value;
            _pixelCollider.pixels = value.SectionPixels;
        }
    }
    public string SortingLayer {
        get => _textureRenderer.sortingLayer;
        set => _textureRenderer.sortingLayer = value;
    }
    public int OrderInLayer {
        get => _textureRenderer.orderInLayer;
        set => _textureRenderer.orderInLayer = value;
    }
    public Color Color {
        get => _textureRenderer.Color;
        set => _textureRenderer.Color = value;
    }
    public bool FlipX {
        get => _textureRenderer.flipX;
        set {
            _textureRenderer.flipX = value;
            _pixelCollider.flipX = value;
        }
    }

    public bool FlipY {
        get => _textureRenderer.flipY;
        set {
            _textureRenderer.flipY = value;
            _pixelCollider.flipY = value;
        }
    }

    private readonly TextureRenderer _textureRenderer;
    private readonly PixelCollider _pixelCollider;
    
    public TextureCollider(bool isActive, ColliderState state, TextureRenderer tr) : base(isActive) {
        IsActive = isActive;
        _textureRenderer = tr;
        _pixelCollider = new PixelCollider(true, tr.texture.SectionPixels, tr.flipX, tr.flipY, state);
    }

    public override void Awake() {
        AddComponent(_textureRenderer);
        AddComponent(_pixelCollider);
    }
}