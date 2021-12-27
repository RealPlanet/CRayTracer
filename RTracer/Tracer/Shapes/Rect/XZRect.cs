using RTracer.Tracer.Utility;
using RTracer.World.Materials;
using System;

namespace RTracer.Tracer.Hittables
{
    using Point3 = Vector3;

    class XZRect : Hittable
    {
        public XZRect() { }
        public XZRect(double _x0, double _x1, double _z0, double _z1, double _k, Material mat)
        {
            x0 = _x0;
            x1 = _x1;
            z0 = _z0;
            z1 = _z1;
            k = _k;
            mp = mat;
        }

        public override bool BoundingBox(double time0, double time1, ref AABB output_box)
        {
            output_box = new AABB(new Point3(x0, k - 0.0001, z0), new Point3(x1, k + 0.0001, z1));
            return true;
        }

        public override bool Hit(ref Ray Ray, double t_min, double t_max, ref HitInfo HitRecord)
        {
            double t = (k - Ray.Origin.y) / Ray.Direction.y;
            if (t < t_min || t > t_max)
                return false;
            double x = Ray.Origin.x + t * Ray.Direction.x;
            double z = Ray.Origin.z + t * Ray.Direction.z;
            if (x < x0 || x > x1 || z < z0 || z > z1)
                return false;
            HitRecord.u = (x - x0) / (x1 - x0);
            HitRecord.v = (z - z0) / (z1 - z0);
            HitRecord.Delta = t;
            Vector3 outward_normal = new Vector3(0, 1, 0);
            HitRecord.SetFaceNormal(Ray, outward_normal);
            HitRecord.Material = mp;
            HitRecord.Point = Ray.At(t);
            return true;
        }

        public override double PDFValue(Point3 origin, Vector3 v)
        {
            HitInfo HitRecord = new HitInfo();
            Ray Ray = new Ray(origin, v);
            if (!Hit(ref Ray, 0.001, double.PositiveInfinity, ref HitRecord))
                return 0;

            var area = (x1 - x0) * (z1 - z0);
            var distance_squared = HitRecord.Delta * HitRecord.Delta * v.LengthSquared();
            var cosine = Math.Abs(v.Dot(HitRecord.Normal)) / v.Length();

            return distance_squared / (cosine * area);
        }

        public override Vector3 Random(Point3 origin)
        {
            var random_point = new Point3(VMath.RandomDouble(x0, x1), k, VMath.RandomDouble(z0, z1));
            return random_point - origin;
        }

        Material mp;
        double x0 = 0.0f;
        double x1 = 0.0f;
        double z0 = 0.0f;
        double z1 = 0.0f;
        double k = 0.0f;
    }
}
