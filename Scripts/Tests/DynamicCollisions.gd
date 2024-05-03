extends BakeBlendMesh

var the_mesh: BakeBlendMesh
var collision: CollisionShape3D

# Called when the node enters the scene tree for the first time.
func _ready():
	the_mesh = get_node(".")
	collision = get_node(("StaticBody3D/CollisionShape3D"))
	_loop_with_delay(0.1)
	pass # Replace with function body.

func _update_baked_mesh():
	var _baked_mesh: ArrayMesh = the_mesh.bake_mesh_from_current_blend_shape_mix()
	var _new_shape: ConcavePolygonShape3D = _baked_mesh.create_trimesh_shape()
	collision.set_shape(_new_shape)

func _loop_with_delay(delay: float):
	while true:
		_update_baked_mesh()
		await get_tree().create_timer(delay).timeout

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
