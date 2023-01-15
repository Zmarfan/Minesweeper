﻿using Worms.engine.core.renderer;
using Worms.engine.data;
using Worms.engine.game_object.components.texture_renderer;

namespace Worms.engine.game_object.components.particle_system.renderer; 

public class RendererBuilder {
    private readonly Texture _texture;
    private string _sortingLayer = TextureRendererHandler.DEFAULT_SORTING_LAYER;
    private int _sortOrder = 0;
    private bool _flipX;
    private bool _flipY;

    public RendererBuilder(Texture texture) {
        _texture = texture;
    }

    public static RendererBuilder Builder(Texture texture) {
        return new RendererBuilder(texture);
    }

    public Renderer Build() {
        return new Renderer(_texture, _sortingLayer, _sortOrder, _flipX, _flipY);
    }

    public RendererBuilder SetSortingLayer(string sortingLayer) {
        _sortingLayer = sortingLayer;
        return this;
    }
    
    public RendererBuilder SetSortingOrder(int sortingOrder) {
        _sortOrder = sortingOrder;
        return this;
    }

    public RendererBuilder SetFlipX(bool flipX) {
        _flipX = flipX;
        return this;
    }
    
    public RendererBuilder SetFlipY(bool flipY) {
        _flipY = flipY;
        return this;
    }
}