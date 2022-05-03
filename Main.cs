using System;
using Godot;

public class Main : Node2D
{
    private readonly PackedScene _boxScene = (PackedScene)GD.Load("res://block.tscn");
    private int _nodeNameCounter = 0;
    private Vector2 _windowSize;

    private Camera2D _camera2D;
    private RigidBody2D _currentBlock;
    
    private Node2D _ropeNode;
    private Vector2 _ropeSpriteSize;
    private float _halfHeightRope;
    
    public override void _Ready()
    {
        _windowSize = GetViewport().GetVisibleRect().Size;
        _camera2D = GetNode<Camera2D>("Camera");
        
        _ropeNode = GetNode<Node2D>("Rope");
        _ropeSpriteSize = _ropeNode.GetNode<Sprite>("Sprite").Texture.GetSize();
        _halfHeightRope = _ropeSpriteSize.y / 2;
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
                boxNode.GetNode("RigidBody2D").GetNode<CollisionShape2D>("CollisionShape2D").Scale =
                    new Vector2(0.5f, 0.5f);

                _currentBlock = boxNode.GetNode<RigidBody2D>("RigidBody2D");
            }

            _nodeNameCounter++;
        }

        if (_currentBlock != null)
        {
            if (_currentBlock.GetCollidingBodies().Count > 0)
            {
                if (_nodeNameCounter > 1 && !(bool) _currentBlock.Get("SkipBlock"))
                {
                    _camera2D.Position = new Vector2(_camera2D.Position.x, _camera2D.Position.y - 196.0f);
                    _currentBlock.Set("SkipBlock", true);
                }
            }
        }

        FollowRopeToPoint(GetGlobalMousePosition());
    }

    public void FollowRopeToPoint(Vector2 trackingPoint)
    {
        Vector2 topCenterPoint = new Vector2(_windowSize.x / 2, -GetGlobalTransformWithCanvas().origin.y - 80);
        float oppositeDirectionToTrackingPoint = (trackingPoint - topCenterPoint).Angle() - Mathf.Deg2Rad(180);
        float distToTrackingPoint = Mathf.Clamp(_halfHeightRope - topCenterPoint.DistanceTo(trackingPoint), 0, _halfHeightRope);
        
        Vector2 initialOffset = new Vector2((float) Math.Cos(oppositeDirectionToTrackingPoint), 
            (float) Math.Sin(oppositeDirectionToTrackingPoint)) * distToTrackingPoint;
        
        _ropeNode.Position = initialOffset + topCenterPoint;
        _ropeNode.Rotation = (trackingPoint - topCenterPoint).Angle() - Mathf.Deg2Rad(90);
    }
}
