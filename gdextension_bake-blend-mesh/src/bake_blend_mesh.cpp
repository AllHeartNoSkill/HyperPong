#include "bake_blend_mesh.h"

using namespace godot;

Ref<ArrayMesh> BakeBlendMesh::bake_mesh_from_current_blend_shape_mix(Ref<ArrayMesh> p_existing)
{
	Ref<ArrayMesh> source_mesh = get_mesh();
	ERR_FAIL_NULL_V_MSG(source_mesh, Ref<ArrayMesh>(), "The source mesh must be a valid ArrayMesh.");

	Ref<ArrayMesh> bake_mesh;

	if (p_existing.is_valid())
	{
		ERR_FAIL_NULL_V_MSG(p_existing, Ref<ArrayMesh>(), "The existing mesh must be a valid ArrayMesh.");
		ERR_FAIL_COND_V_MSG(source_mesh == p_existing, Ref<ArrayMesh>(), "The source mesh can not be the same mesh as the existing mesh.");

		bake_mesh = p_existing;
	}
	else
	{
		bake_mesh.instantiate();
	}

	Mesh::BlendShapeMode blend_shape_mode = source_mesh->get_blend_shape_mode();
	int mesh_surface_count = source_mesh->get_surface_count();

	bake_mesh->clear_surfaces();
	bake_mesh->set_blend_shape_mode(blend_shape_mode);

	for (int surface_index = 0; surface_index < mesh_surface_count; surface_index++)
	{
		uint32_t surface_format = source_mesh->surface_get_format(surface_index);

		ERR_CONTINUE(0 == (surface_format & Mesh::ARRAY_FORMAT_VERTEX));

		const Array &source_mesh_arrays = source_mesh->surface_get_arrays(surface_index);

		ERR_FAIL_COND_V(source_mesh_arrays.size() != Mesh::ARRAY_MAX, Ref<ArrayMesh>());

		const PackedVector3Array &source_mesh_vertex_array = source_mesh_arrays[Mesh::ARRAY_VERTEX];
		const PackedVector3Array &source_mesh_normal_array = source_mesh_arrays[Mesh::ARRAY_NORMAL];
		const PackedFloat32Array &source_mesh_tangent_array = source_mesh_arrays[Mesh::ARRAY_TANGENT];

		Array new_mesh_arrays;
		new_mesh_arrays.resize(Mesh::ARRAY_MAX);
		for (int i = 0; i < source_mesh_arrays.size(); i++)
		{
			if (i == Mesh::ARRAY_VERTEX || i == Mesh::ARRAY_NORMAL || i == Mesh::ARRAY_TANGENT)
			{
				continue;
			}
			new_mesh_arrays[i] = source_mesh_arrays[i];
		}

		bool use_normal_array = source_mesh_normal_array.size() == source_mesh_vertex_array.size();
		bool use_tangent_array = source_mesh_tangent_array.size() / 4 == source_mesh_vertex_array.size();

		PackedVector3Array lerped_vertex_array = source_mesh_vertex_array;
		PackedVector3Array lerped_normal_array = source_mesh_normal_array;
		PackedFloat32Array lerped_tangent_array = source_mesh_tangent_array;

		const Vector3 *source_vertices_ptr = source_mesh_vertex_array.ptr();
		const Vector3 *source_normals_ptr = source_mesh_normal_array.ptr();
		const float *source_tangents_ptr = source_mesh_tangent_array.ptr();

		Vector3 *lerped_vertices_ptrw = lerped_vertex_array.ptrw();
		Vector3 *lerped_normals_ptrw = lerped_normal_array.ptrw();
		float *lerped_tangents_ptrw = lerped_tangent_array.ptrw();

		const Array &blendshapes_mesh_arrays = source_mesh->surface_get_blend_shape_arrays(surface_index);
		int blend_shape_count = source_mesh->get_blend_shape_count();
		ERR_FAIL_COND_V(blendshapes_mesh_arrays.size() != blend_shape_count, Ref<ArrayMesh>());

		for (int blendshape_index = 0; blendshape_index < blend_shape_count; blendshape_index++)
		{
			float blend_weight = get_blend_shape_value(blendshape_index);
			// if (abs(blend_weight) <= 0.0001)
			// {
			// 	continue;
			// }

			const Array &blendshape_mesh_arrays = blendshapes_mesh_arrays[blendshape_index];

			const PackedVector3Array &blendshape_vertex_array = blendshape_mesh_arrays[Mesh::ARRAY_VERTEX];
			const PackedVector3Array &blendshape_normal_array = blendshape_mesh_arrays[Mesh::ARRAY_NORMAL];
			const PackedFloat32Array &blendshape_tangent_array = blendshape_mesh_arrays[Mesh::ARRAY_TANGENT];

			ERR_FAIL_COND_V(source_mesh_vertex_array.size() != blendshape_vertex_array.size(), Ref<ArrayMesh>());
			ERR_FAIL_COND_V(source_mesh_normal_array.size() != blendshape_normal_array.size(), Ref<ArrayMesh>());
			ERR_FAIL_COND_V(source_mesh_tangent_array.size() != blendshape_tangent_array.size(), Ref<ArrayMesh>());

			const Vector3 *blendshape_vertices_ptr = blendshape_vertex_array.ptr();
			const Vector3 *blendshape_normals_ptr = blendshape_normal_array.ptr();
			const float *blendshape_tangents_ptr = blendshape_tangent_array.ptr();

			if (blend_shape_mode == Mesh::BLEND_SHAPE_MODE_NORMALIZED)
			{
				for (int i = 0; i < source_mesh_vertex_array.size(); i++)
				{
					const Vector3 &source_vertex = source_vertices_ptr[i];
					const Vector3 &blendshape_vertex = blendshape_vertices_ptr[i];
					Vector3 lerped_vertex = source_vertex.lerp(blendshape_vertex, blend_weight) - source_vertex;
					lerped_vertices_ptrw[i] += lerped_vertex;

					if (use_normal_array)
					{
						const Vector3 &source_normal = source_normals_ptr[i];
						const Vector3 &blendshape_normal = blendshape_normals_ptr[i];
						Vector3 lerped_normal = source_normal.lerp(blendshape_normal, blend_weight) - source_normal;
						lerped_normals_ptrw[i] += lerped_normal;
					}

					if (use_tangent_array)
					{
						int tangent_index = i * 4;
						const Vector4 source_tangent = Vector4(
							source_tangents_ptr[tangent_index],
							source_tangents_ptr[tangent_index + 1],
							source_tangents_ptr[tangent_index + 2],
							source_tangents_ptr[tangent_index + 3]);
						const Vector4 blendshape_tangent = Vector4(
							blendshape_tangents_ptr[tangent_index],
							blendshape_tangents_ptr[tangent_index + 1],
							blendshape_tangents_ptr[tangent_index + 2],
							blendshape_tangents_ptr[tangent_index + 3]);
						Vector4 lerped_tangent = source_tangent.lerp(blendshape_tangent, blend_weight);
						lerped_tangents_ptrw[tangent_index] += lerped_tangent.x;
						lerped_tangents_ptrw[tangent_index + 1] += lerped_tangent.y;
						lerped_tangents_ptrw[tangent_index + 2] += lerped_tangent.z;
						lerped_tangents_ptrw[tangent_index + 3] += lerped_tangent.w;
					}
				}
			}
			else if (blend_shape_mode == Mesh::BLEND_SHAPE_MODE_RELATIVE)
			{
				for (int i = 0; i < source_mesh_vertex_array.size(); i++)
				{
					const Vector3 &blendshape_vertex = blendshape_vertices_ptr[i];
					lerped_vertices_ptrw[i] += blendshape_vertex * blend_weight;

					if (use_normal_array)
					{
						const Vector3 &blendshape_normal = blendshape_normals_ptr[i];
						lerped_normals_ptrw[i] += blendshape_normal * blend_weight;
					}

					if (use_tangent_array)
					{
						int tangent_index = i * 4;
						const Vector4 blendshape_tangent = Vector4(
							blendshape_tangents_ptr[tangent_index],
							blendshape_tangents_ptr[tangent_index + 1],
							blendshape_tangents_ptr[tangent_index + 2],
							blendshape_tangents_ptr[tangent_index + 3]);
						Vector4 lerped_tangent = blendshape_tangent * blend_weight;
						lerped_tangents_ptrw[tangent_index] += lerped_tangent.x;
						lerped_tangents_ptrw[tangent_index + 1] += lerped_tangent.y;
						lerped_tangents_ptrw[tangent_index + 2] += lerped_tangent.z;
						lerped_tangents_ptrw[tangent_index + 3] += lerped_tangent.w;
					}
				}
			}
		}

		new_mesh_arrays[Mesh::ARRAY_VERTEX] = lerped_vertex_array;
		if (use_normal_array)
		{
			new_mesh_arrays[Mesh::ARRAY_NORMAL] = lerped_normal_array;
		}
		if (use_tangent_array)
		{
			new_mesh_arrays[Mesh::ARRAY_TANGENT] = lerped_tangent_array;
		}

		bake_mesh->add_surface_from_arrays(Mesh::PRIMITIVE_TRIANGLES, new_mesh_arrays, Array(), Dictionary(), surface_format);
	}

	return bake_mesh;
}

void BakeBlendMesh::_bind_methods()
{
	ClassDB::bind_method(D_METHOD("bake_mesh_from_current_blend_shape_mix", "p_existing"), &BakeBlendMesh::bake_mesh_from_current_blend_shape_mix, DEFVAL(Ref<ArrayMesh>()));
}

BakeBlendMesh::BakeBlendMesh()
{
	// Initialize any variables here.
}

BakeBlendMesh::~BakeBlendMesh()
{
	// Add your cleanup here.
}
