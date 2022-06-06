extends Node2D

onready var Admob : AdMob = get_parent().get_node("AdMob")

func _ready():
	Admob.load_banner()

func _on_AdMob_banner_loaded():
	_set_value_offset_camera()
	Admob.show_banner();

func _set_value_offset_camera():
	get_parent().get_node("Rope").set("_banner_dimension", Admob.get_banner_dimension())
	get_parent().get_node("Rope").set("_cameraMoved", false)
