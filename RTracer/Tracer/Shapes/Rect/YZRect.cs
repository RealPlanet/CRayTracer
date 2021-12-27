using RTracer.Tracer;
using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;
using RTracer.World.Materials;

namespace RTraceRay.TraceRay.Hittables
{
    using Point3 = Vector3;
    class YZRect : Hittable
    {
        public YZRect() { }
        public YZRect(double _y0, double _y1, double _z0, double _z1, double _k, Material mat)
        {
            y0 = _y0;
            y1 = _y1;
            z0 = _z0;
            z1 = _z1;
            k = _k;
            mp = mat;
        }

        public override bool BoundingBox(double time0, double time1, ref AABB output_box)
        {
            // The bounding box must have non-zero width in each dimension, so pad the X
            // dimension a small amount.
            output_box = new AABB(new Point3(k - 0.0001, y0, z0), new Point3(k + 0.0001, y1, z1));
            return true;
        }
        public override bool Hit(ref Ray Ray, double t_min, double t_max, ref HitInfo HitRecord)
        {
            double t = (k - Ray.Origin.x) / Ray.Direction.x;
            if (t < t_min || t > t_max)
                return false;
            double y = Ray.Origin.y + t * Ray.Direction.y;
            double z = Ray.Origin.z + t * Ray.Direction.z;

            if (y < y0 || y > y1 || z < z0 || z > z1)
                return false;
            HitRecord.u = (y - y0) / (y1 - y0);
            HitRecord.v = (z - z0) / (z1 - z0);
            HitRecord.Delta = t;
            Vector3 outward_normal = new(1, 0, 0);
            HitRecord.SetFaceNormal(Ray, outward_normal);
            HitRecord.Material = mp;
            HitRecord.Point = Ray.At(t);
            return true;
        }

        Material mp;
        double z0 = 0.0f;
        double z1 = 0.0f;
        double y0 = 0.0f;
        double y1 = 0.0f;
        double k = 0.0f;
    }
}
