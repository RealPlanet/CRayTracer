#pragma once
#include "Scene.h"
class EarthScene :
    public Scene
{
public:
    EarthScene()
    {
        world = earth();
        background = Color(0.70, 0.80, 1.00);
        lookfrom = Point3(13, 2, 3);
        lookat = Point3(0, 0, 0);
        vfov = 20.0;
        samples_per_pixel = 500;
    }
private:
    HittableList earth()
    {
        auto earth_texture = make_shared<ImageTexture>("_images/earthmap.jpg");
        auto earth_surface = make_shared<Lambertian>(earth_texture);
        auto globe = make_shared<Sphere>(Point3(0, 0, 0), 2, earth_surface);

        return HittableList(globe);
    }

};

