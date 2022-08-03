#pragma once

#include <Utility/Vector3.h>
#include <Hittable/HittableList.h>
#include <Textures/Texture.h>
#include <Materials/Material.h>
#include <Shapes/Sphere.h>
#include <Shapes/Rect.h>

class Scene
{
public:
	Color background = Color(0, 0, 0);
	Point3 lookfrom = Point3(0, 0, 0);
	Point3 lookat = Point3(0, 0, 0);
	double vfov = 40.0;
	double aperture = 0.0;
	HittableList world = HittableList();
	shared_ptr<HittableList> lights = make_shared<HittableList>();

	int samples_per_pixel = 400;
	int max_depth = 10;
	Scene()
	{
		
	}
};

