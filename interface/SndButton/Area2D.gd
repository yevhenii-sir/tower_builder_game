extends Area2D

var animation : AnimationPlayer
var main : Node2D
var isPressed = false;
var onSound = true;

func _ready():
	animation = get_node("AnimationPlayer");
	main = get_parent().get_parent().get_parent().get_parent();

func start_animation():
	animation.play("FadeUp")

func start_back_animation():
	animation.play_backwards("FadeUp")
	
func refresh_color_btn():
	if onSound:
		$background.modulate = Color("f71b1b");
		if !main.get_node("backgroundAudioPlayer").playing:
			main.get_node("backgroundAudioPlayer").playing = true;
	else:
		$background.modulate = Color.lightseagreen;
		main.get_node("backgroundAudioPlayer").playing = false;
		
func start_play_music():
	if (onSound && !main.get_node("backgroundAudioPlayer").playing):
		main.get_node("backgroundAudioPlayer").playing = true;

func _on_Area2D_input_event(viewport, event, shape_idx):
	if !animation.is_playing():
		if (event is InputEventMouseButton && event.pressed):
			animation.play("pressAnim");
			if onSound:
				onSound = false
				refresh_color_btn()
			else:
				onSound = true
				refresh_color_btn()
