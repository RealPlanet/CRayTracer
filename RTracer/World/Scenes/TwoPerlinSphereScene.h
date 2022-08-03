#pragma once
#include "Scene.h"
class TwoPerlinSphereScene :
    public Scene
{
public:
    TwoPerlinSphereScene()
    {
        world = two_perlin_spheres();
        background = Color(0.70, 0.80, 1.00);
        lookfrom = Point3(13, 2, 3);
        lookat = Point3(0, 0, 0);
        vfov = 20.0;
        samples_per_pixel = 500;
    }
private:
    HittableList two_perlin_spheres()
    {
        HittableList objects;

        auto pertext = make_shared<NoiseTexture>(4);
        objects.add(make_shared<Sphere>(Point3(0, -1000, 0), 1000, make_shared<Lambertian>(pertext)));
        objects.add(make_shared<Sphere>(Point3(0, 2, 0), 2, make_shared<Lambertian>(pertext)));

        return objects;
    }

};

