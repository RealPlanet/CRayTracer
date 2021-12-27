using RTracer.Tracer.Hittables;
using RTracer.World.Materials;
using System;
using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;
using RTracer.World.Materials;
using RTracer.World.Textures;
using System;
using static RTracer.Tracer.Utility.VMath;

namespace RTracer.Tracer.Shapes
{
    using Color = Vector3;
    using Point3 = Vector3;
    class MovingSphere : Hittable
    {
        public MovingSphere()
        {
            center0 = new Point3(0, 0, 0);
            center1 = new Point3(0, 0, 0);
            time0 = 0;
            time0 = 0;
            radius = 1;
            mat_ptr = new Lambertian(new Color(1,1,1));
        }
        public MovingSphere(Point3 cen0, Point3 cen1, double _time0, double _time1, double r, Material m)
        {
            center0 = cen0;
            center1 = cen1;
            time0 = _time0;
            time1 = _time1;
            radius = r;
            mat_ptr = m;
        }

        public override bool BoundingBox(double Time0, double Time1, ref AABB OutputBox)
        {
            AABB box0 = new(center(Time0) -new Vector3(radius, radius, radius), center(Time0) + new Vector3(radius, radius, radius));
            AABB box1 = new(center(Time1) -new Vector3(radius, radius, radius), center(Time1) + new Vector3(radius, radius, radius));
            OutputBox = AABB.SurroundingBox(box0, box1);
            return true;
        }

        public override bool Hit(ref Ray Ray, double TimeMin, double TimeMax, ref HitInfo HitRecord)
        {
            Vector3 oc = Ray.Origin - center(Ray.Time);
            double a = Ray.Direction.LengthSquared();
            double half_b = oc.Dot( Ray.Direction);
            double c = oc.LengthSquared() - radius * radius;

            double discriminant = half_b * half_b - a * c;
            if (discriminant < 0) return false;
            double sqrtd = Math.Sqrt(discriminant);

            // Find the nearest root that lies in the acceptable range.
            double root = (-half_b - sqrtd) / a;
            if (root < TimeMin || TimeMax < root)
            {
                root = (-half_b + sqrtd) / a;
                if (root < TimeMin || TimeMax < root)
                {
                    return false;
                }
            }

            HitRecord.Delta = root;
            HitRecord.Point = Ray.At(HitRecord.Delta);
            var outward_normal = (HitRecord.Point - center(Ray.Time)) / radius;
            HitRecord.SetFaceNormal(Ray, outward_normal);
            HitRecord.Material = mat_ptr;

            return true;
        }

        public Vector3 center(double time)
        {
            return center0 + ((time - time0) / (time1 - time0)) * (center1 - center0);
        }

        Point3 center0, center1;
        double time0 = 0.0f;
        double time1 = 0.0f;
        double radius = 0.0f;
        Material mat_ptr;
    }
}
