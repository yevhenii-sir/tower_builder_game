using System;
using Godot;

public class Main : Node2D
{
    private readonly PackedScene _boxScene = (PackedScene)GD.Load("res://block.tscn");
    private int _nodeNameCounter = 0;
    private Vector2 _windowSize;

    private Camera2D _camera2D;
    private RigidBody2D _currentBlock;
    
    public override void _Ready()
    {
        _windowSize = GetViewport().GetVisibleRect().Size;
        _camera2D = GetNode<Camera2D>("Camera");
    }
    
    public override void _Process(float delta)
    {
        if (OS.GetName() == "Windows" && Input.IsActionPressed("restart_game"))
            GetTree().ReloadCurrentScene();

        if (Input.IsActionJustPressed("mouse_left_pressed"))
        {
            Node box = _boxScene.Instance();
            box.Name = "box_" + _nodeNameCounter;
            AddChild(box);

            var boxNode = GetNode<Node2D>("box_" + _nodeNameCounter);
            if (boxNode != null)
            {
                boxNode.Position = GetGlobalMousePosition();
                boxNode.GetNode("RigidBody2D").GetNode<Sprite>("Sprite").Scale = new Vector2(0.5f, 0.5f);
                boxNode.GetNode("RigidBody2D").GetNode<CollisionShape2D>("CollisionShape2D").Scale = new Vector2(0.5f, 0.5f);

                _currentBlock = boxNode.GetNode<RigidBody2D>("RigidBody2D");
            }
            
            _nodeNameCounter++;
        }

        if (_currentBlock != null)
        {
            if (_currentBlock.GetCollidingBodies().Count > 0)
            {
                if (_nodeNameCounter > 1 && !(bool)_currentBlock.Get("SkipBlock"))
                {
                    _camera2D.Position = new Vector2(_camera2D.Position.x, _camera2D.Position.y - 196.0f);
                    _currentBlock.Set("SkipBlock", true);
                }
            }
        }
    }
}
