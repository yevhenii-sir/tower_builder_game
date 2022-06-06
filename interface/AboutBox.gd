extends Area2D

var animation : AnimationPlayer
var main : Node2D
var current_aplha = 0
var prew_animation = "";

func _ready():
	animation = get_node("AnimationPlayer");
	main = get_parent().get_parent().get_parent().get_parent();
	get_parent().position.x = get_viewport_rect().size.x / 2
	get_parent().position.y = get_viewport_rect().size.y / 2

func start_animation():
	animation.stop(true)
	animation.play("FadeUp")

func start_back_animation():
	animation.play_backwards("FadeUp")

func _on_Area2D_input_event(viewport, event, shape_idx):
	if !animation.is_playing():
		if (event is InputEventMouseButton && event.pressed):
			animation.play("FadeDown");
			
func _process(delta):
	if animation.current_animation != "":
		prew_animation = animation.current_animation;
		
	if prew_animation == "FadeUp":
		current_aplha = lerp(current_aplha, 0.5, 0.09);
	else:
		current_aplha = lerp(current_aplha, 0, 0.09);
