using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;
using RTracer.World.Materials;
using RTraceRay.TraceRay.Hittables;

namespace RTracer.Tracer.Shapes
{
    using Point3 = Vector3;

    class Box : Hittable
    {
        public Point3 box_min = new Point3(0, 0, 0);
        public Point3 box_max = new Point3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        public HittableList sides = new HittableList();

        public Box(Point3 p0, Point3 p1, Material ptr)
        {
            box_min = p0;
            box_max = p1;

            sides.Add(new XYRect(p0.x, p1.x, p0.y, p1.y, p1.z, ptr));
            sides.Add(new XYRect(p0.x, p1.x, p0.y, p1.y, p0.z, ptr));

            sides.Add(new XZRect(p0.x, p1.x, p0.z, p1.z, p1.y, ptr));
            sides.Add(new XZRect(p0.x, p1.x, p0.z, p1.z, p0.y, ptr));

            sides.Add(new YZRect(p0.y, p1.y, p0.z, p1.z, p1.x, ptr));
            sides.Add(new YZRect(p0.y, p1.y, p0.z, p1.z, p0.x, ptr));
        }

        public override bool BoundingBox(double time0, double time1, ref AABB output_box)
        {
            output_box = new AABB(box_min, box_max);
            return true;
        }

        public override bool Hit(ref Ray Ray, double t_min, double t_max, ref HitInfo HitRecord)
        {
            return sides.Hit(ref Ray, t_min, t_max, ref HitRecord);
        }
    }
}
