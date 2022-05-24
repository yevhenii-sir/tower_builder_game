using System.Drawing;
using Godot;

public class Camera : Camera2D
{
    public override void _Ready()
    {
        GoToStartupPosition();
    }
    
    public override void _Process(float delta)
    {
        if (OS.GetName() == "Windows")
        {
            if (Input.IsActionPressed("camera_up"))
                Position = new Vector2(Position.x, Position.y - 12);

            if (Input.IsActionPressed("camera_down"))
                Position = new Vector2(Position.x, Position.y + 12);
        }
    }

    public void GoToStartupPosition()
    {
        Vector2 banner_width = (Vector2)GetParent().GetNode<Node2D>("Rope").Get("_banner_dimension");
        Position = new Vector2(0, 0 - (GetViewport().GetVisibleRect().Size.y - 1024) + (banner_width.y / 2));
    }
}
