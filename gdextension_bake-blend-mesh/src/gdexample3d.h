#ifndef GDEXAMPLE3D_H
#define GDEXAMPLE3D_H

#include <godot_cpp/classes/mesh_instance3d.hpp>

namespace godot
{

	class GDExample3D : public MeshInstance3D
	{
		GDCLASS(GDExample3D, MeshInstance3D)

	private:
		double time_passed;

	protected:
		static void _bind_methods();

	public:
		GDExample3D();
		~GDExample3D();

		void _process(double delta) override;
	};
}

#endif
