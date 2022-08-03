#pragma once
#include "Scene.h"
class SimpleLightScene :
    public Scene
{
public:
    SimpleLightScene()
    {
        world = simple_light();
        samples_per_pixel = 400;
        background = Color(0, 0, 0);
        lookfrom = Point3(26, 3, 6);
        lookat = Point3(0, 2, 0);
        vfov = 20.0;
    }
private:
    HittableList simple_light()
    {
        HittableList objects;

        auto pertext = make_shared<NoiseTexture>(4);
        objects.add(make_shared<Sphere>(Point3(0, -1000, 0), 1000, make_shared<Lambertian>(pertext)));
        objects.add(make_shared<Sphere>(Point3(0, 2, 0), 2, make_shared<Lambertian>(pertext)));

        auto difflight = make_shared<DiffuseLight>(Color(4, 4, 4));
        objects.add(make_shared<XYRect>(3, 5, 1, 3, -2, difflight));
        objects.add(make_shared<Sphere>(Point3(0, 5, 0), 2, difflight));

        return objects;
    }
};

