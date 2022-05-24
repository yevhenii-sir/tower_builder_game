extends Area2D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var animation : AnimationPlayer
var main : Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	animation = get_node("AnimationPlayer");
	#animation.play("Flow")
	
	main = get_parent().get_parent().get_parent().get_parent();
	#animation.stop();
	#animation.seek(0, true)
	#animation.stop();


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func start_animation():
	animation.play("Flow")

func _on_Area2D_input_event(viewport, event, shape_idx):
	if !animation.is_playing():
		if (event is InputEventMouseButton && event.pressed):
			animation.play_backwards("Flow")
			main.set("_isStart", false)
