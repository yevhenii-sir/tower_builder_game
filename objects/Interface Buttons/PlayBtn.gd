extends Area2D

var animation : AnimationPlayer
var main : Node2D
var isPressed = false;

func _ready():
	animation = get_node("AnimationPlayer");
	main = get_parent().get_parent().get_parent().get_parent();

func start_animation():
	animation.play("Flow")

func _on_Area2D_input_event(viewport, event, shape_idx):
	if !animation.is_playing():
		if (event is InputEventMouseButton && event.pressed):
			animation.play_backwards("Flow")
			main.set("_isStart", false)
			main.set("nowPressedPlayBtn", true)
