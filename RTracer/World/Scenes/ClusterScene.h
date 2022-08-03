#pragma once
#include "Scene.h"
#include <Hittable/ConstantMedium.h>
#include <Shapes/MovingSphere.h>
#include <BVHNode/BVHNode.h>
#include <Shapes/Box.h>

class ClusterScene :
    public Scene
{
public:
    ClusterScene()
    {
        samples_per_pixel = 50;
        background = Color(0, 0, 0);
        lookfrom = Point3(478, 278, -600);
        lookat = Point3(278, 278, 0);
        vfov = 40.0;
        world = final_scene();
    }
private:
    HittableList final_scene()
    {
        HittableList objects;
        auto ground = make_shared<Lambertian>(Color(0.48, 0.83, 0.53));

        const int boxes_per_side = 20;
        for (int i = 0; i < boxes_per_side; i++)
        {
            for (int j = 0; j < boxes_per_side; j++)
            {
                auto w = 100.0;
                auto x0 = -1000.0 + i * w;
                auto z0 = -1000.0 + j * w;
                auto y0 = 0.0;
                auto x1 = x0 + w;
                auto y1 = random_double(1, 101);
                auto z1 = z0 + w;

                objects.add(make_shared<Box>(Point3(x0, y0, z0), Point3(x1, y1, z1), ground));
            }
        }

        objects.add(make_shared<BVHNode>(objects, 0, 1));

        auto light = make_shared<DiffuseLight>(Color(7, 7, 7));
        objects.add(make_shared<XZRect>(123, 423, 147, 412, 554, light));

        auto center1 = Point3(400, 400, 200);
        auto center2 = center1 + Vector3(30, 0, 0);
        auto moving_sphere_material = make_shared<Lambertian>(Color(0.7, 0.3, 0.1));
        objects.add(make_shared<MovingSphere>(center1, center2, 0, 1, 50, moving_sphere_material));

        objects.add(make_shared<Sphere>(Point3(260, 150, 45), 50, make_shared<Dielectric>(1.5)));
        objects.add(make_shared<Sphere>(
            Point3(0, 150, 145), 50, make_shared<Metal>(Color(0.8, 0.8, 0.9), 1.0)
            ));

        auto boundary = make_shared<Sphere>(Point3(360, 150, 145), 70, make_shared<Dielectric>(1.5));
        objects.add(boundary);
        objects.add(make_shared<ConstantMedium>(boundary, 0.2, Color(0.2, 0.4, 0.9)));
        boundary = make_shared<Sphere>(Point3(0, 0, 0), 5000, make_shared<Dielectric>(1.5));
        objects.add(make_shared<ConstantMedium>(boundary, .0001, Color(1, 1, 1)));

        auto emat = make_shared<Lambertian>(make_shared<ImageTexture>("_images/earthmap.jpg"));
        objects.add(make_shared<Sphere>(Point3(400, 200, 400), 100, emat));
        auto pertext = make_shared<NoiseTexture>(0.1);
        objects.add(make_shared<Sphere>(Point3(220, 280, 300), 80, make_shared<Lambertian>(pertext)));

        auto white = make_shared<Lambertian>(Color(.73, .73, .73));
        int ns = 1000;
        for (int j = 0; j < ns; j++) {
            objects.add(make_shared<Sphere>(Point3::random(0, 165), 10, white));
        }

        objects.add(make_shared<Translate>(make_shared<RotateY>(make_shared<BVHNode>(objects, 0.0, 1.0), 15), Vector3(-100, 270, 395)));

        return objects;
    }

};

