#ifndef BAKE_BLEND_MESH_H
#define BAKE_BLEND_MESH_H

#include <godot_cpp/classes/mesh_instance3d.hpp>
#include <godot_cpp/classes/array_mesh.hpp>
#include <godot_cpp/classes/mesh_instance3d.hpp>
#include <godot_cpp/core/class_db.hpp>
#include <godot_cpp/classes/collision_shape3d.hpp>
#include <godot_cpp/classes/static_body3d.hpp>
#include <godot_cpp/classes/skeleton3d.hpp>
#include <godot_cpp/classes/concave_polygon_shape3d.hpp>
#include <godot_cpp/classes/convex_polygon_shape3d.hpp>

namespace godot
{

	class BakeBlendMesh : public MeshInstance3D
	{
		GDCLASS(BakeBlendMesh, MeshInstance3D)

	private:
	protected:
		Ref<Mesh> mesh;
		static void _bind_methods();

	public:
		Ref<ArrayMesh> bake_mesh_from_current_blend_shape_mix(Ref<ArrayMesh> p_existing);

		BakeBlendMesh();
		~BakeBlendMesh();
	};
}

#endif
