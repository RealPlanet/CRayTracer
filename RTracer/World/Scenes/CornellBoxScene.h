#pragma once
#include "Scene.h"
class CornellBoxScene :
    public Scene
{
   
public:
    CornellBoxScene()
    {
        world = cornell_box();
        samples_per_pixel = 100;
        lookfrom = Point3(278, 278, -800);
        lookat = Point3(278, 278, 0);
        vfov = 40.0;
        max_depth = 50;
    }
private:
    HittableList cornell_box()
    {
        HittableList objects;

        auto red   = make_shared<Lambertian>(Color(.65, .05, .05));
        auto white = make_shared<Lambertian>(Color(.73, .73, .73));
        auto green = make_shared<Lambertian>(Color(.12, .45, .15));
        auto light = make_shared<DiffuseLight>(Color(20, 20, 20));

        objects.add(make_shared<YZRect>(0, 555, 0, 555, 555, green));
        objects.add(make_shared<YZRect>(0, 555, 0, 555, 0, red));
        objects.add(make_shared<XZRect>(0, 555, 0, 555, 555, white));
        objects.add(make_shared<XZRect>(0, 555, 0, 555, 0, white));
        objects.add(make_shared<XYRect>(0, 555, 0, 555, 555, white));
        auto light_ = make_shared<FlipFace>(make_shared<XZRect>(213, 343, 227, 332, 554, light));
        objects.add(light_);
        lights->add(light_->ptr);

        shared_ptr<Hittable> box1 = make_shared<Box>(Point3(0,0,0), Point3(165,330,165), white);
		shared_ptr<Material> aluminum = make_shared<Metal>(Color(0.8, 0.85, 0.88), 0.0);
        box1 = make_shared<RotateY>(box1, 15);
        box1 = make_shared<Translate>(box1, Vector3(265,0,295));
        objects.add(box1);

		auto glass = make_shared<Dielectric>(1.5);
		objects.add(make_shared<Sphere>(Point3(190, 90, 190), 90, glass));
        return objects;
    }

};

