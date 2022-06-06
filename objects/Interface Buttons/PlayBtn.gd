extends Area2D

var animation : AnimationPlayer
var main : Node2D
var isPressed = false
var soundBtn : Area2D

func _ready():
	animation = get_node("AnimationPlayer");
	soundBtn = get_parent().get_parent().get_node("SoundBtn").get_node("Area2D")
	main = get_parent().get_parent().get_parent().get_parent();
	get_parent().position.x = get_viewport_rect().size.x / 2

func start_animation():
	animation.play("Flow")

func _on_Area2D_input_event(viewport, event, shape_idx):
	if !animation.is_playing():
		if (event is InputEventMouseButton && event.pressed):
			animation.play_backwards("Flow")
			main.set("_isStart", false)
			main.set("nowPressedPlayBtn", true)
			soundBtn.call("start_back_animation");
			
