using RTracer.Tracer.Utility;
using RTracer.World.Materials;

namespace RTracer.Tracer.Hittables
{
    using Point3 = Vector3;
    class XYRect : Hittable
    {
        public XYRect() { }
        public XYRect(double _x0, double _x1, double _y0, double _y1, double _k, Material mat)
        {
            x0 = _x0;
            x1 = _x1;
            y0 = _y0;
            y1 = _y1;
            k = _k;
            mp = mat;
        }

        public override bool BoundingBox(double time0, double time1, ref AABB output_box)
        {
            output_box = new AABB(new Point3(x0, y0, k - 0.0001), new Point3(x1, y1, k + 0.0001));
            return true;
        }

        public override bool Hit(ref Ray Ray, double t_min, double t_max, ref HitInfo HitRecord)
        {
            double t = (k - Ray.Origin.z) / Ray.Direction.z;
            if (t < t_min || t > t_max)
                return false;
            double x = Ray.Origin.x + t * Ray.Direction.x;
            double y = Ray.Origin.y + t * Ray.Direction.y;
            if (x < x0 || x > x1 || y < y0 || y > y1)
                return false;
            HitRecord.u = (x - x0) / (x1 - x0);
            HitRecord.v = (y - y0) / (y1 - y0);
            HitRecord.Delta = t;
            Vector3 outward_normal = new Vector3(0, 0, 1);
            HitRecord.SetFaceNormal(Ray, outward_normal);
            HitRecord.Material = mp;
            HitRecord.Point = Ray.At(t);
            return true;
        }

        Material mp;
        double x0 = 0.0f;
        double x1 = 0.0f;
        double y0 = 0.0f;
        double y1 = 0.0f;
        double k = 0.0f;
    }
}
