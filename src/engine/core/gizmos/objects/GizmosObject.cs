﻿using Worms.engine.data;

namespace Worms.engine.core.gizmos.objects; 

public abstract class GizmosObject {
    public readonly Color color;

    protected GizmosObject(Color color) {
        this.color = color;
    }

    public abstract void Render(nint renderer, TransformationMatrix worldToScreenMatrix);
}