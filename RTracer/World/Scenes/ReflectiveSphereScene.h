#pragma once
#include "Scene.h"
class ReflectiveSphereScene :
	public Scene
{
public:
	ReflectiveSphereScene()
	{
		background = Color(0.70, 0.80, 1.00);
		lookfrom = Point3(13, 2, 3);
		lookat = Point3(0, 0, 0);
		vfov = 20.0;
		aperture = 0.1;
		samples_per_pixel = 100;
		world = cornell_box();
	}
private:
	HittableList cornell_box()
	{
		HittableList objects;
		auto red = make_shared<Lambertian>(Color(.65, .05, .05));
		auto white = make_shared<Lambertian>(Color(.73, .73, .73));
		auto green = make_shared<Lambertian>(Color(.12, .45, .15));
		auto light = make_shared<DiffuseLight>(Color(15, 15, 15));

		objects.add(make_shared<YZRect>(0, 555, 0, 555, 555, green));
		objects.add(make_shared<YZRect>(0, 555, 0, 555, 0, red));
		objects.add(make_shared<XZRect>(0, 555, 0, 555, 555, white));
		objects.add(make_shared<XZRect>(0, 555, 0, 555, 0, white));
		objects.add(make_shared<XYRect>(0, 555, 0, 555, 555, white));

		shared_ptr<Material> mat = make_shared<Metal>(Color(0,0,0), 0.0);
		objects.add(make_shared<Sphere>(Point3(190, 90, 190), 90, light));
		return objects;
	}
};



