using RTracer.Tracer.Hittables;
using RTracer.Tracer.Shapes;
using RTracer.Tracer.Utility;
using RTracer.World.Materials;
using RTraceRay.TraceRay.Hittables;

namespace RTracer.World.Scenes
{
    using Color = Vector3;
    using Point3 = Vector3;

    class CornellBoxScene : Scene
    {
        public CornellBoxScene()
        {
            ObjectList = BuildScene();
            PixelSamples = 65;
            lookfrom = new Point3(278, 278, -800);
            lookat = new Point3(278, 278, 0);
            vfov = 40.0;
            RayBounces = 15;
        }

        private HittableList BuildScene()
        {
            HittableList List = new();

            Material RedMaterial = new Lambertian(new Color(.65, .05, .05));
            Material WhiteMaterial = new Lambertian(new Color(.73, .73, .73));
            Material GreenMaterial = new Lambertian(new Color(.12, .45, .15));
            Material EmissiveMaterial = new DiffuseLight(new Color(15, 15, 15));
            Material GlassMaterial = new Dielectric(1.5);
            Material Aluminum = new Metal(new Color(0.8, 0.85, 0.88), 0.0);

            List.Add(new YZRect(0, 555, 0, 555, 555, GreenMaterial));
            List.Add(new YZRect(0, 555, 0, 555, 0, RedMaterial));
            List.Add(new XZRect(0, 555, 0, 555, 555, WhiteMaterial));
            List.Add(new XZRect(0, 555, 0, 555, 0, WhiteMaterial));
            List.Add(new XYRect(0, 555, 0, 555, 555, WhiteMaterial));

            FlipFace LightBox = new FlipFace(new XZRect(213, 343, 227, 332, 554, EmissiveMaterial));
            Hittable RotatedBox = new Box(new Point3(0, 0, 0), new Point3(165, 330, 165), Aluminum);

            // TODO :: For some reason Translation my happen before rotation
            RotatedBox = new Translate(RotatedBox, new Point3(265, 0, 295));
            RotatedBox = new RotateY(RotatedBox, 15);

            List.Add(LightBox);
            LightList.Add(LightBox.HitObject);

            // TODO :: For some reason adding anything after the RotatedBox translates and rotates those objects too.
            List.Add(new Sphere(new Point3(190, 90, 190), 90, GlassMaterial));
            List.Add(RotatedBox);

            return List;
        }
    }
}
