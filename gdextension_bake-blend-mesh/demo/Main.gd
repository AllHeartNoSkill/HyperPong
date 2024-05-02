extends Node3D

var mesh_instance: BakeBlendMesh
var collision: CollisionShape3D
var shape: ConcavePolygonShape3D

# Called when the node enters the scene tree for the first time.
func _ready():
	mesh_instance = get_node("BakeBlendMesh")
	collision = get_node("BakeBlendMesh/StaticBody3D/CollisionShape3D")
	var _current_shape = collision.shape.get_faces()
	print(mesh_instance.get_blend_shape_value(0))
	# for i in _current_shape.size():
	# 	print(_current_shape[i])
	var _updated_mesh: Mesh = mesh_instance.bake_mesh_from_current_blend_shape_mix()
	var _array = _updated_mesh.get_faces()
	print("assigning to collision")
	collision.shape = shape
	print("hecc")
	for i in _array.size():
		print(_array[i])

	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
