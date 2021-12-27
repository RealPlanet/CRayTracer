using AForge.Imaging.Filters;
using RTracer.Tracer;
using RTracer.Tracer.Hittables;
using RTracer.Tracer.Shapes;
using RTracer.Tracer.Utility;
using RTracer.World.Materials;
using RTraceRay.TraceRay.Hittables;
using System.Drawing;


namespace RTracer.World.Scenes
{
    using Color = Vector3;
    using Point3 = Vector3;
    class ExtremelyComplexScene : Scene
    {
        public ExtremelyComplexScene()
        {
            PixelSamples = 60;
            RayBounces = 25;
            lookfrom = new Point3(278, 278, -800);
            lookat = new Point3(278, 278, 0);
            vfov = 40.0;
            ObjectList = BuildScene();
        }
        
        private HittableList BuildScene()
        {
            // TODO :: Implement These
            
            HittableList objects = new();
            HittableList boxes1 = new();
            HittableList boxes2 = new();

            var ground = new Lambertian(new Color(0.48, 0.83, 0.53));

            const int boxes_per_side = 20;
            for (int i = 0; i < boxes_per_side; i++)
            {
                for (int j = 0; j < boxes_per_side; j++)
                {
                    var w = 100.0;
                    var x0 = -1000.0 + i * w;
                    var z0 = -1000.0 + j * w;
                    var y0 = 0.0;
                    var x1 = x0 + w;
                    var y1 = VMath.RandomDouble(1, 101);
                    var z1 = z0 + w;

                    boxes1.Add(new Box(new Point3(x0, y0, z0), new Point3(x1, y1, z1), ground));
                }
            }
            objects.Add(new BVHNode(boxes1, 0, 1));

            var EmissiveMaterial = new DiffuseLight(new Color(15, 15, 15));
            FlipFace LightBox = new FlipFace(new XZRect(213, 343, 227, 332, 554, EmissiveMaterial));
            objects.Add(LightBox);
            LightList.Add(LightBox.HitObject);

            var center1 = new Point3(400, 400, 200);
            var center2 = center1 + new Vector3(30, 0, 0);
            var moving_sphere_material = new Lambertian(new Color(0.7, 0.3, 0.1));
            objects.Add(new MovingSphere(center1, center2, 0, 1, 50, moving_sphere_material));

            objects.Add(new Sphere(new Point3(260, 150, 45), 50, new Dielectric(1.5)));
            objects.Add(new Sphere(
                new Point3(0, 150, 145), 50, new Metal(new Color(0.8, 0.8, 0.9), 1.0)
            ));

            var boundary = new Sphere(new Point3(360, 150, 145), 70, new Dielectric(1.5));
            objects.Add(boundary);
            objects.Add(new ConstantMedium(boundary, 0.2, new Color(0.2, 0.4, 0.9)));
            boundary = new Sphere(new Point3(0, 0, 0), 5000, new Dielectric(1.5));
            objects.Add(new ConstantMedium(boundary, .0001, new Color(1, 1, 1)));

            //var emat = new Lambertian(new ImageTexture("earthmap.jpg"));
            //objects.Add(new Sphere(new Point3(400, 200, 400), 100, emat));
            //var pertext = new NoiseTexture(0.1);
            //objects.Add(new Sphere(new Point3(220, 280, 300), 80, new Lambertian(pertext)));

            
            var white = new Lambertian(new Color(.73, .73, .73));
            int ns = 1000;
            for (int j = 0; j < ns; j++)
            {
                boxes2.Add(new Sphere(Vector3.Random(0, 165), 10, white));
            }

            objects.Add(new Translate(
                    new RotateY(
                        new BVHNode(boxes2, 0.0, 1.0), 15),
                        new Vector3(-100, 270, 395))
            );

            return objects;
        }
    }
}
