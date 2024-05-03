#ifndef BAKE_BLEND_MESH_H
#define BAKE_BLEND_MESH_H

#include <godot_cpp/classes/mesh_instance3d.hpp>
#include <godot_cpp/classes/array_mesh.hpp>
#include <godot_cpp/templates/local_vector.hpp>
#include <godot_cpp/variant/variant.hpp>

namespace godot
{

	class BakeBlendMesh : public MeshInstance3D
	{
		GDCLASS(BakeBlendMesh, MeshInstance3D)

	private:
	protected:
		static void _bind_methods();

	public:
		Ref<ArrayMesh> bake_mesh_from_current_blend_shape_mix(Ref<ArrayMesh> p_existing);

		BakeBlendMesh();
		~BakeBlendMesh();
	};
}

#endif
