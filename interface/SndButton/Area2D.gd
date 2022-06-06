extends Area2D

var animation : AnimationPlayer
var main : Node2D
var isPressed = false;
var onSound = true;

func _ready():
	animation = get_node("AnimationPlayer");
	main = get_parent().get_parent().get_parent().get_parent();
	#animation.play("FadeUp")

func start_animation():
	animation.play("FadeUp")

func start_back_animation():
	animation.play_backwards("FadeUp")

func _on_Area2D_input_event(viewport, event, shape_idx):
	if !animation.is_playing():
		if (event is InputEventMouseButton && event.pressed):
			animation.play("pressAnim");
			if onSound:
				$background.modulate = Color.lightseagreen;
				onSound = false
			else:
				$background.modulate = Color("f71b1b");
				onSound = true
			#main.set("_isStart", false)
			#main.set("nowPressedPlayBtn", true)
