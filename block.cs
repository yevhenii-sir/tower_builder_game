using Godot;
using System;

public class block : Node2D
{
	private bool _isShowing = true;
	
	public override void _Ready()
	{
		Scale = new Vector2(0, 0);
	}
	
	public override void _Process(float delta)
	{
		if (_isShowing)
		{
			var scale = Mathf.Lerp(Scale.x, 1f, 0.2f);
			Scale = new Vector2(scale, scale);

			if (Math.Abs(scale - 1) < 0.01) _isShowing = false;
		}
	}
}
