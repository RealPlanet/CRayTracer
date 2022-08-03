#pragma once
#include "Scene.h"
class RandomScene :
    public Scene
{
public:
    RandomScene()
    {
        background = Color(0.70, 0.80, 1.00);
        lookfrom = Point3(13, 2, 3);
        lookat = Point3(0, 0, 0);
        vfov = 20.0;
        aperture = 0.1;
        samples_per_pixel = 500;
        world = random_scene();
    }
private:
    HittableList random_scene()
    {
        HittableList world;

        auto checker = make_shared<CheckerTexture>(Color(0.2, 0.3, 0.1), Color(0.9, 0.9, 0.9));
        world.add(make_shared<Sphere>(Point3(0, -1000, 0), 1000, make_shared<Lambertian>(checker)));

        for (int a = -11; a < 11; a++)
        {
            for (int b = -11; b < 11; b++)
            {
                auto choose_mat = random_double();
                Point3 center(a + 0.9 * random_double(), 0.2, b + 0.9 * random_double());

                if ((center - Point3(4, 0.2, 0)).length() > 0.9) {
                    shared_ptr<Material> sphere_material;

                    if (choose_mat < 0.8)
                    {
                        // diffuse
                        auto albedo = Color::random() * Color::random();
                        sphere_material = make_shared<Lambertian>(albedo);
                        auto center2 = center + Vector3(0, random_double(0, .5), 0);
                        world.add(make_shared<MovingSphere>(center, center2, 0.0, 1.0, 0.2, sphere_material));
                    }
                    else if (choose_mat < 0.95)
                    {
                        // Metal
                        auto albedo = Color::random(0.5, 1);
                        auto fuzz = random_double(0, 0.5);
                        sphere_material = make_shared<Metal>(albedo, fuzz);
                        world.add(make_shared<Sphere>(center, 0.2, sphere_material));
                    }
                    else {
                        // glass
                        sphere_material = make_shared<Dielectric>(1.5);
                        world.add(make_shared<Sphere>(center, 0.2, sphere_material));
                    }
                }
            }
        }

        auto material1 = make_shared<Dielectric>(1.5);
        world.add(make_shared<Sphere>(Point3(0, 1, 0), 1.0, material1));

        auto material2 = make_shared<Lambertian>(Color(0.4, 0.2, 0.1));
        world.add(make_shared<Sphere>(Point3(-4, 1, 0), 1.0, material2));

        auto material3 = make_shared<Metal>(Color(0.7, 0.6, 0.5), 0.0);
        world.add(make_shared<Sphere>(Point3(4, 1, 0), 1.0, material3));

        return world;
    }
};



