using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;
using RTracer.World.Materials;
using RTracer.World.Textures;
using System;
using static RTracer.Tracer.Utility.VMath;

namespace RTracer.Tracer.Shapes
{
    using Color = Vector3;
    class ConstantMedium : Hittable
    {
        public Hittable Boundary;
        public Material PhaseFunction;
        double NegInvDensity;

        public ConstantMedium(Hittable b, double d, Texture a)
        {
            Boundary = b;
            NegInvDensity = -1 / d;
            PhaseFunction = new Isotropic(a);
        }

        public ConstantMedium(Hittable b, double d, Color c)
        {
            Boundary = b;
            NegInvDensity = -1 / d;
            PhaseFunction = new Isotropic(c);
        }

        public override bool BoundingBox(double time0, double time1, ref AABB output_box) => Boundary.BoundingBox(time0, time1, ref output_box);

        public override bool Hit(ref Ray Ray, double t_min, double t_max, ref HitInfo HitRecord)
        {
            // Print occasional samples when debugging. To enable, set enableDebug true.
            const bool enableDebug = false;

            HitInfo rec1 = new();
            HitInfo rec2 = new();

            if (!Boundary.Hit(ref Ray, double.NegativeInfinity, double.PositiveInfinity, ref rec1))
                return false;

            if (!Boundary.Hit(ref Ray, rec1.Delta + 0.0001d, double.PositiveInfinity, ref rec2))
                return false;

            if (enableDebug)
                Console.WriteLine($"t_min = {rec1.Delta}\nt_max = {rec2.Delta}");

            if (rec1.Delta < t_min) rec1.Delta = t_min;
            if (rec2.Delta > t_max) rec2.Delta = t_max;

            if (rec1.Delta >= rec2.Delta)
                return false;

            if (rec1.Delta < 0)
                rec1.Delta = 0;

            double ray_length = Ray.Direction.Length();
            double distance_inside_Boundary = (rec2.Delta - rec1.Delta) * ray_length;
            double hit_distance = NegInvDensity * Math.Log(RandomDouble());

            if (hit_distance > distance_inside_Boundary)
                return false;

            HitRecord.Delta = rec1.Delta + hit_distance / ray_length;
            HitRecord.Point = Ray.At(HitRecord.Delta);

            if (enableDebug)
            {
                Console.WriteLine($"hit_distance = {hit_distance}\nrec.t = {HitRecord.Delta}\nrec.point ={HitRecord.Point}");
            }

            HitRecord.Normal = new Vector3(1, 0, 0);  // arbitrary
            HitRecord.FrontFace = true;     // also arbitrary
            HitRecord.Material = PhaseFunction;

            return true;
        }
    }
}
