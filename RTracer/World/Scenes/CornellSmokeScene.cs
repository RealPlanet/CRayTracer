using RTracer.Tracer.Hittables;
using RTracer.Tracer.Shapes;
using RTracer.Tracer.Utility;
using RTracer.World.Materials;
using RTraceRay.TraceRay.Hittables;

namespace RTracer.World.Scenes
{
    using Color = Vector3;
    using Point3 = Vector3;
    class CornellSmokeScene : Scene
    {
        public CornellSmokeScene()
        {
            PixelSamples = 60;
            RayBounces = 30;
            lookfrom = new Point3(278, 278, -800);
            lookat = new Point3(278, 278, 0);
            vfov = 40.0;
            ObjectList = BuildScene();
        }

        private HittableList BuildScene()
        {
            HittableList objects = new();

            var red = new Lambertian(new Color(.65, .05, .05));
            var white = new Lambertian(new Color(.73, .73, .73));
            var green = new Lambertian(new Color(.12, .45, .15));
            var EmissiveMaterial = new DiffuseLight(new Color(15, 15, 15));

            objects.Add(new YZRect(0, 555, 0, 555, 555, green));
            objects.Add(new YZRect(0, 555, 0, 555, 0, red));

            FlipFace LightBox = new FlipFace(new XZRect(213, 343, 227, 332, 554, EmissiveMaterial));
            objects.Add(LightBox);
            LightList.Add(LightBox.HitObject);

            objects.Add(new XZRect(0, 555, 0, 555, 555, white));
            objects.Add(new XZRect(0, 555, 0, 555, 0, white));
            objects.Add(new XYRect(0, 555, 0, 555, 555, white));

            Hittable box1 = new Box(new Point3(0, 0, 0), new Point3(165, 330, 165), white);
            Hittable box2 = new Box(new Point3(0, 0, 0), new Point3(165, 165, 165), white);

            // TODO :: Rotating seems to cause issues
            box1 = new Translate(box1, new Vector3(265, 0, 295));
            //box1 = new RotateY(box1, 15);
            box2 = new Translate(box2, new Vector3(130, 0, 65));
            //box2 = new RotateY(box2, -18);
            
            objects.Add(new ConstantMedium(box1, 0.01, new Color(0, 0, 0)));
            objects.Add(new ConstantMedium(box2, 0.01, new Color(1, 1, 1)));

            return objects;
        }
    }
}
