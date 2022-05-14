using Godot;

public class Camera : Camera2D
{
    public override void _Ready()
    {
        Position = new Vector2(Position.x, Position.y - (GetViewport().GetVisibleRect().Size.y - 1024));
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
}
