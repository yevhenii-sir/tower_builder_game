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
	
		private readonly PackedScene _boxScene = (PackedScene)GD.Load("res://block.tscn");
		private int _nodeNameCounter = 0;
		private Node2D _currentBox;

		private Timer _timerSpawnBox;

		public override void _Ready()
		{
			_leftClaw = GetNode<Sprite>("claw_left");
			_rightClaw = GetNode<Sprite>("claw_right");
			_timerSpawnBox = GetNode<Timer>("timer_box_spawn");

			if (_leftClaw != null && _rightClaw != null && _timerSpawnBox != null)
			{
				_handlerMoveClaws = MoveClawsDown;
				_on_timer_box_spawn_timeout();
			}
			else 
				GetTree().ReloadCurrentScene();
		}
	
		public override void _Process(float delta)
		{
			if (_currentBox != null)
			{
				if (Input.IsActionJustPressed("mouse_left_pressed"))
				{
					var currentBoxRigidBody2D = _currentBox.GetNode<RigidBody2D>("RigidBody2D");
					currentBoxRigidBody2D.Mode = RigidBody2D.ModeEnum.Rigid;
				
					var pos = _currentBox.GlobalPosition;
					var angle = _currentBox.GlobalRotation;
					RemoveChild(_currentBox);
					GetParent().AddChild(_currentBox);
					_currentBox.Position = pos;
					_currentBox.Rotation = angle;
					_currentBox.Scale = new Vector2(1, 1);
					_currentBox = null;
				
					_timerSpawnBox.Start();
				
					ClearAllDelegates();
					_handlerMoveClaws = MoveClawsUp;
				}
				else TrackPositionBox(_currentBox);
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

		/// <summary>
		/// Спавн коробки в клещах по окончанию таймера
		/// </summary>
		public void _on_timer_box_spawn_timeout()
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
