using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;
using RTracer.World.Materials;
using System;

namespace RTracer.Tracer.Shapes
{
    using Point3 = Vector3;

    class Sphere : Hittable
    {
        public Point3 center;
        public double radius = 0.0f;
        public Material Material;
        public Sphere()
        {
            center = new Point3(0, 0, 0);
        }

        public Sphere(Point3 cen, double r, Material m)
        {
            center = cen;
            radius = r;
            Material = m;
        }

        public override bool BoundingBox(double time0, double time1, ref AABB output_box)
        {
            Vector3 CVec = new Vector3(radius, radius, radius);
            output_box = new AABB(center - CVec, center + CVec);

            return true;
        }

        public override bool Hit(ref Ray Ray, double t_min, double t_max, ref HitInfo HitRecord)
        {
            /*
               Calculate numbers to solve this quadratic equation
               (t^2)n*n+2tn*(A−C)+(A−C)*(A−C)−r^2=0
               discriminant = (b2−4ac)
               t == constant that rapresents the current position on the ray [0, 1]
               r == sphere radius
               A == vector ray origin
               C == vector sphere origin
               n == vector ray direction

               a = n * n
               b = 2 * n * (A - C)
               c = (A-C)^2 - r^2
            */
            Vector3 oc_vec = Ray.Origin - center; //Vector that passes the ray origin and the sphere center
            double a = Ray.Direction.LengthSquared(); // dot == squared lenght
            double half_b = oc_vec.Dot(Ray.Direction); // b == 2h => simplifies the equation
            double c = oc_vec.LengthSquared() - (radius * radius);

            double discriminant = (half_b * half_b) - (a * c);
            if (discriminant < 0)
                return false;

            double sqrtd = Math.Sqrt(discriminant);

            // Find the nearest root that lies in the acceptable range.
            double root = (-half_b - sqrtd) / a;
            if (root < t_min || t_max < root)
            {
                root = (-half_b + sqrtd) / a;
                if (root < t_min || t_max < root)
                    return false;
            }

            HitRecord.Delta = root;
            HitRecord.Point = Ray.At(HitRecord.Delta);
            Vector3 outward_normal = (HitRecord.Point - center) / radius;
            HitRecord.SetFaceNormal(Ray, outward_normal);
            get_sphere_uv(outward_normal, HitRecord.u, HitRecord.v);
            HitRecord.Material = Material;

            return true;
        }

        public void get_sphere_uv(Point3 Point, double u, double v)
        {
            // Point: a given point on the sphere of radius one, centered at the origin.
            // u: returned value [0,1] of angle around the Y axis from X=-1.
            // v: returned value [0,1] of angle from Y=-1 to Y=+1.
            //     <1 0 0> yields <0.50 0.50>       <-1  0  0> yields <0.00 0.50>
            //     <0 1 0> yields <0.50 1.00>       < 0 -1  0> yields <0.50 0.00>
            //     <0 0 1> yields <0.25 0.50>       < 0  0 -1> yields <0.75 0.50>

            var theta = Math.Acos(-Point.y);
            var phi = Math.Atan2(-Point.z, Point.x) + Math.PI;
            u = phi / (2 * Math.PI);
            v = theta / Math.PI;
        }

        public override double PDFValue(Point3 o, Point3 v)
        {
            HitInfo HitRecord = new HitInfo();
            Ray Ray = new Ray(o, v);
            if (!Hit(ref Ray, 0.001, double.PositiveInfinity, ref HitRecord))
                return 0;

            var cos_theta_max = Math.Sqrt(1 - radius * radius / (center - o).LengthSquared());
            var solid_angle = 2 * Math.PI * (1 - cos_theta_max);

            return 1 / solid_angle;
        }

        public override Point3 Random(Point3 o)
        {
            Vector3 direction = center - o;
            var distance_squared = direction.LengthSquared();
            ONB uvw = new ONB();
            uvw.build_from_w(direction);
            return uvw.Local(random_to_sphere(radius, distance_squared));
        }

        public static Vector3 random_to_sphere(double radius, double distance_squared)
        {
            var r1 = VMath.RandomDouble();
            var r2 = VMath.RandomDouble();
            var z = 1 + r2 * (Math.Sqrt(1 - radius * radius / distance_squared) - 1);

            var phi = 2 * Math.PI * r1;
            var x = Math.Cos(phi) * Math.Sqrt(1 - z * z);
            var y = Math.Sin(phi) * Math.Sqrt(1 - z * z);

            return new Vector3(x, y, z);
        }
    }
}
