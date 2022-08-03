#pragma once
#include "Scene.h"
class TwoSphereScene :
    public Scene
{
public:
    TwoSphereScene()
    {
        world = two_spheres();
        background = Color(0.70, 0.80, 1.00);
        lookfrom = Point3(13, 2, 3);
        lookat = Point3(0, 0, 0);
        vfov = 20.0;
        samples_per_pixel = 10;
    }
private:
    HittableList two_spheres() {
        HittableList objects;

        auto checker = make_shared<CheckerTexture>(Color(0.2, 0.3, 0.1), Color(0.9, 0.9, 0.9));

        objects.add(make_shared<Sphere>(Point3(0, -10, 0), 10, make_shared<Lambertian>(checker)));
        objects.add(make_shared<Sphere>(Point3(0, 10, 0), 10, make_shared<Lambertian>(checker)));

        return objects;
    }
};

