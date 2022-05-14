#define DEBUG

using System;
using Godot;

namespace TowerBuilder
{
	public class rope_claws : Node2D
	{
		private Sprite _leftClaw;
		private Sprite _rightClaw;
	
		private float _startRotationRad = 0;
		private readonly float _finishRotationRad = Mathf.Deg2Rad(45);
		private float _increaseValAngle = 0.30f;

		private delegate void MoveClaws();
		private MoveClaws _handlerMoveClaws;
	
		private readonly PackedScene _boxScene = (PackedScene)GD.Load("res://objects/Block/block.tscn");
		private int _nodeNameCounter = 0;
		private bool _boxFly = false;
		private Node2D _currentBox;

		private Camera2D _camera2D;
		private Label _labelHeightCounter;
		public override void _Ready()
		{
			_leftClaw = GetNode<Sprite>("claw_left");
			_rightClaw = GetNode<Sprite>("claw_right");
			_camera2D = GetParent().GetNode<Camera2D>("Camera");
			_labelHeightCounter = GetParent()
				.GetNode<Camera2D>("Camera")
				.GetNode<CanvasLayer>("CanvasLayer")
				.GetNode<Control>("Interface").GetNode<NinePatchRect>("TowerHeight")
				.GetNode<Label>("CounterText");

			if (_leftClaw != null && _rightClaw != null && _camera2D != null)
			{
				_handlerMoveClaws = MoveClawsDown;
				SpawnBox();
			}
			else 
				GetTree().ReloadCurrentScene();
		}
	
		public override void _Process(float delta)
		{
			if (_currentBox != null)
			{
				if (!_boxFly)
				{
					if (Input.IsActionJustPressed("mouse_left_pressed"))
					{
						var currentBoxRigidBody2D = _currentBox.GetNode<RigidBody2D>("RigidBody2D");
						currentBoxRigidBody2D.Mode = RigidBody2D.ModeEnum.Rigid;
						currentBoxRigidBody2D.GravityScale = 15;

						var pos = _currentBox.GlobalPosition;
						var angle = _currentBox.GlobalRotation;
						RemoveChild(_currentBox);
						GetParent().AddChild(_currentBox);
						_currentBox.Position = pos;
						_currentBox.Rotation = angle;
						_currentBox.Scale = new Vector2(1, 1);
						_boxFly = true;
						
						_currentBox.Set("_isStabilization", true);

						ClearAllDelegates();
						_handlerMoveClaws = MoveClawsUp;
					}
					else TrackPositionBox(_currentBox);
				}
				else
				{
					if (_currentBox.GetNode<RigidBody2D>("RigidBody2D").GetCollidingBodies().Count > 0)
					{
						var tempScore = (int) GetParent().Get("_score");
						var rigidBodyBox = _currentBox.GetNode<RigidBody2D>("RigidBody2D");

						if (tempScore > 0)
						{
							_camera2D.Position = new Vector2(_camera2D.Position.x,
								(rigidBodyBox.GlobalTransform.origin.y) -
								_camera2D.GetViewportRect().Size.y + 200);
						}


						rigidBodyBox.Set("SkipBlock", true);
						_currentBox.Set("_isStabilization", false);
						GetParent().Set("_score", tempScore + 1);

						_labelHeightCounter.Text = (tempScore + 1).ToString();

						rigidBodyBox.ContactMonitor = false;
						_boxFly = false;
						_currentBox = null;
						
						SpawnBox();
					}
				}
			}

			_handlerMoveClaws?.Invoke();
		
#if DEBUG
			if (Input.IsActionJustPressed("claws_up"))
			{
				ClearAllDelegates();
				_handlerMoveClaws = MoveClawsUp;
			} else if (Input.IsActionJustPressed("claws_down"))
			{
				ClearAllDelegates();
				_handlerMoveClaws = MoveClawsDown;
			}
#endif
		}

		private void MoveClawsUp()
		{
			_leftClaw.Rotation = Mathf.LerpAngle(_leftClaw.Rotation, _finishRotationRad, _increaseValAngle);
			_rightClaw.Rotation = Mathf.LerpAngle(_rightClaw.Rotation, -_finishRotationRad, _increaseValAngle);

			if (Math.Abs(_leftClaw.Rotation - _finishRotationRad) < 0.02) _handlerMoveClaws -= MoveClawsUp;
		}
	
		private void MoveClawsDown()
		{
			_leftClaw.Rotation = Mathf.LerpAngle(_leftClaw.Rotation, _startRotationRad, _increaseValAngle);
			_rightClaw.Rotation = Mathf.LerpAngle(_rightClaw.Rotation, _startRotationRad, _increaseValAngle);
		
			if (Math.Abs(_leftClaw.Rotation - _startRotationRad) < 0.02) _handlerMoveClaws -= MoveClawsDown;
		}
	
		private void ClearAllDelegates()
		{
			if (_handlerMoveClaws != null)
			{
				foreach (Delegate dDelegate in _handlerMoveClaws.GetInvocationList())
					_handlerMoveClaws -= (MoveClaws) dDelegate;
			}
		}
	
		private void TrackPositionBox(Node2D box)
		{
			box.Position = _leftClaw.Position + new Vector2(0, 40);
		}
		
		public void SpawnBox()
		{
			Node box = _boxScene.Instance();
			box.Name = "box_" + _nodeNameCounter;
			AddChild(box);

			_currentBox = GetNode<Node2D>("box_" + _nodeNameCounter);
			if (_currentBox != null)
			{
				TrackPositionBox(_currentBox);
				_currentBox.GetNode("RigidBody2D").GetNode<Sprite>("Sprite").Scale = new Vector2(0.5f, 0.5f);
				_currentBox.GetNode("RigidBody2D").GetNode<CollisionShape2D>("CollisionShape2D").Scale =
					new Vector2(0.5f, 0.5f);

				var currentBoxRigidBody2D = _currentBox.GetNode<RigidBody2D>("RigidBody2D");
				currentBoxRigidBody2D.Mode = RigidBody2D.ModeEnum.Static;
			
				ClearAllDelegates();
				_handlerMoveClaws = MoveClawsDown;
			}

			_nodeNameCounter++;
		}
	}
}