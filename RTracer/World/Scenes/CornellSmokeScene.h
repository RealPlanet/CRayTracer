#pragma once
#include "Scene.h"
class CornellSmokeScene :
    public Scene
{
public:
    CornellSmokeScene()
    {
        samples_per_pixel = 100;
        lookfrom = Point3(278, 278, -800);
        lookat = Point3(278, 278, 0);
        vfov = 40.0;
        world = cornell_smoke();
    }
private:
    HittableList cornell_smoke()
    {
        HittableList objects;

        auto red = make_shared<Lambertian>(Color(.65, .05, .05));
        auto white = make_shared<Lambertian>(Color(.73, .73, .73));
        auto green = make_shared<Lambertian>(Color(.12, .45, .15));
        auto light = make_shared<DiffuseLight>(Color(7, 7, 7));

        objects.add(make_shared<YZRect>(0, 555, 0, 555, 555, green));
        objects.add(make_shared<YZRect>(0, 555, 0, 555, 0, red));
        objects.add(make_shared<XZRect>(113, 443, 127, 432, 554, light));
        objects.add(make_shared<XZRect>(0, 555, 0, 555, 555, white));
        objects.add(make_shared<XZRect>(0, 555, 0, 555, 0, white));
        objects.add(make_shared<XYRect>(0, 555, 0, 555, 555, white));

        shared_ptr<Hittable> box1 = make_shared<Box>(Point3(0, 0, 0), Point3(165, 330, 165), white);
        box1 = make_shared<RotateY>(box1, 15);
        box1 = make_shared<Translate>(box1, Vector3(265, 0, 295));

        shared_ptr<Hittable> box2 = make_shared<Box>(Point3(0, 0, 0), Point3(165, 165, 165), white);
        box2 = make_shared<RotateY>(box2, -18);
        box2 = make_shared<Translate>(box2, Vector3(130, 0, 65));

        objects.add(make_shared<ConstantMedium>(box1, 0.01, Color(0, 0, 0)));
        objects.add(make_shared<ConstantMedium>(box2, 0.01, Color(1, 1, 1)));

        return objects;
    }

};

