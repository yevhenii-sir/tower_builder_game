using Godot;
using System;

public class BacgroundGradient : Line2D
{
    private Vector2 _windowSize;
    public override void _Ready()
    {
        _windowSize = GetViewport().GetVisibleRect().Size;
        Width = _windowSize.x;
    }

    public override void _Process(float delta)
    {
        var origin = GetViewport().GlobalCanvasTransform.origin;
        var centerScreen = _windowSize.x / 2f;
        
        SetPointPosition(0, origin + new Vector2(centerScreen, 0));
        SetPointPosition(1, origin + new Vector2(centerScreen, _windowSize.y));
    }
}
