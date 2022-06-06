extends Node2D

#var nodeArea : Area2D = $Area2D;

func _ready():
	pass # Replace with function body.

func _process(delta):
	update()

func _draw():
	var current_aplha = $Area2D.get("current_aplha");
	var inv = get_global_transform().inverse()
	draw_set_transform(inv.get_origin(), inv.get_rotation(), inv.get_scale())
	
	draw_rect(Rect2(get_viewport_transform().origin, get_viewport_rect().size), Color(0.0, 0, 0, current_aplha))
