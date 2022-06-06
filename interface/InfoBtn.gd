extends Area2D

var animation : AnimationPlayer
var main : Node2D

func _ready():
	animation = get_node("AnimationPlayer");
	main = get_parent().get_parent().get_parent().get_parent();

func start_animation():
	animation.play("FadeUp")

func start_back_animation():
	animation.play_backwards("FadeUp")

func _on_Area2D_input_event(viewport, event, shape_idx):
	if !animation.is_playing():
		if (event is InputEventMouseButton && event.pressed):
			animation.play("pressBtn");
			print("press!")
