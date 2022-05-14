using Godot;
using System;

public class block : Node2D
{
	private bool _isShowing = true;
	public bool _isStabilization = false;

	private RigidBody2D _rigidBody2D;
	
	public override void _Ready()
	{
		Scale = new Vector2(0, 0);
		_rigidBody2D = GetNode<RigidBody2D>("RigidBody2D");
	}
	
	public override void _Process(float delta)
	{
		if (_isShowing)
		{
			var scale = Mathf.Lerp(Scale.x, 1f, 0.2f);
			Scale = new Vector2(scale, scale);

			if (Math.Abs(scale - 1) < 0.01) _isShowing = false;
		}
		
		//GD.Print(Name + " " + Rotation + " " + _rigidBody2D.Rotation);

		if (_isStabilization)
		{
			_rigidBody2D.GlobalRotation = Mathf.Lerp(_rigidBody2D.GlobalRotation, 0, 0.1f);
		}
	}
}
