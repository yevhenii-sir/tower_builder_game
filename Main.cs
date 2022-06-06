using System;
using System.Collections.Generic;
using Godot;

public class Main : Node2D
{
	private Vector2 _windowSize;
	
	private Node2D _ropeNode;
	private Vector2 _ropeSpriteSize;
	private float _halfHeightRope;

	private static float _deg = 2.5f;
	private float _tTimeShiftX, _tTimeShiftY;
	private float _amplitudeX = 100f;
	private float _amplitudeY = 25f;
	private readonly float _incrementX = Mathf.Deg2Rad(_deg);
	private readonly float _incrementY = Mathf.Deg2Rad(_deg) / 2.5f;

	private Vector2 _topCenterPoint;
	private bool _isStart = true;
	private bool _startAnimationPlayBtn = true;

	private Area2D _playBtn;
	private Area2D _soundBtn;
	private Area2D _infoBtn;
	
	public bool nowPressedPlayBtn = false;
	public float _maxScore = 0;
	private Label _labelMaxCounter;

	private int _score = 0;

	public override void _Ready()
	{
		_windowSize = GetViewport().GetVisibleRect().Size;

		_ropeNode = GetNode<Node2D>("Rope");
		_ropeSpriteSize = _ropeNode.GetNode<Sprite>("Sprite").Texture.GetSize();
		_halfHeightRope = _ropeSpriteSize.y / 2;

		StaticBody2D staticBody2D = GetNode<StaticBody2D>("foundament");
		staticBody2D.Position = new Vector2(_windowSize.x / 2, staticBody2D.Position.y);
		
		Node2D background = GetNode<Node2D>("background");
		background.Position = new Vector2((_windowSize.x / 2) - 300, background.Position.y);

		_playBtn = GetNode("Camera")
			.GetNode("CanvasLayer")
			.GetNode("PlayBtn")
			.GetNode<Area2D>("Area2D");
		
		_soundBtn = GetNode("Camera")
			.GetNode("CanvasLayer")
			.GetNode("SoundBtn")
			.GetNode<Area2D>("Area2D");
		
		_infoBtn = GetNode("Camera")
			.GetNode("CanvasLayer")
			.GetNode("InfoButton")
			.GetNode<Area2D>("Area2D");
		
		_labelMaxCounter = GetNode("Camera")
			.GetNode("CanvasLayer")
			.GetNode("Interface")
			.GetNode("TowerHeight")
			.GetNode<Label>("MaxPop");
	}

	public override void _Process(float delta)
	{
		if (OS.GetName() == "Windows" && Input.IsActionPressed("restart_game"))
			GetTree().ReloadCurrentScene();
		
		_topCenterPoint = new Vector2(_windowSize.x / 2, -GetGlobalTransformWithCanvas().origin.y - 80);
		
		FollowRopeToPoint(_topCenterPoint + new Vector2(_amplitudeX * Mathf.Sin(_tTimeShiftX), 
														_amplitudeY * Mathf.Sin(_tTimeShiftY) + 300f));

		_tTimeShiftX += _incrementX;
		_tTimeShiftY += _incrementY;

		if (_startAnimationPlayBtn)
		{
			_startAnimationPlayBtn = false;
			_playBtn.Call("start_animation");
			_soundBtn.Call("start_animation");
			_infoBtn.Call("start_animation");

			LoadGame();
			_soundBtn.Call("start_play_music");
			_labelMaxCounter.Text = "MAX: " + _maxScore.ToString();
			GD.Print("anim_start!!!");
		}
	}
	
	public Godot.Collections.Dictionary<string, object> Save()
	{
		Node mainNode = GetParent().GetParent();
        
		return new Godot.Collections.Dictionary<string, object>()
		{
			{ "MaxScore", _maxScore},
			{ "SoundOn", (bool)_soundBtn.Get("onSound")}
		};
	}
	
	public void LoadGame()
	{
		var saveGame = new File();
		if (!saveGame.FileExists("user://savegame.save"))
			return;
		
		saveGame.Open("user://savegame.save", File.ModeFlags.Read);

		while (saveGame.GetPosition() < saveGame.GetLen())
		{
			var nodeData = new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)JSON.Parse(saveGame.GetLine()).Result);
			foreach (KeyValuePair<string, object> entry in nodeData)
			{
				string key = entry.Key.ToString();

				switch (key)
				{
					case "MaxScore":
						_maxScore = (float)entry.Value;
						break;
					
					case "SoundOn":
						_soundBtn.Set("onSound", (bool)entry.Value);
						_soundBtn.Call("refresh_color_btn");
						break;
				}
				
				GD.Print(key, ": ", entry.Value);
			}
		}

		saveGame.Close();
	}

	private void FollowRopeToPoint(Vector2 trackingPoint)
	{
		float oppositeDirectionToTrackingPoint = (trackingPoint - _topCenterPoint).Angle() - Mathf.Deg2Rad(180);
		float distToTrackingPoint = 
			Mathf.Clamp(_halfHeightRope - _topCenterPoint.DistanceTo(trackingPoint), 0, _halfHeightRope);
		
		Vector2 initialOffset = new Vector2((float) Math.Cos(oppositeDirectionToTrackingPoint), 
			(float) Math.Sin(oppositeDirectionToTrackingPoint)) * distToTrackingPoint;
		
		_ropeNode.Position = initialOffset + _topCenterPoint;
		_ropeNode.Rotation = (trackingPoint - _topCenterPoint).Angle() - Mathf.Deg2Rad(90);
	}
}

