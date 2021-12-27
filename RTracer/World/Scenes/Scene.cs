using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;


namespace RTracer.World.Scenes
{
    using Color = Vector3;
    using Point3 = Vector3;

    class Scene
    {
        public Color BackgroundCol = new Color(0, 0, 0);
        public Point3 lookfrom = new Point3(0, 0, 0);
        public Point3 lookat = new Point3(0, 0, 0);
        public double vfov = 40.0;
        public double aperture = 0.0;
        public HittableList ObjectList = new HittableList();
        public HittableList LightList = new HittableList();

        public int PixelSamples = 50;
        public int RayBounces = 10;
    }
}
