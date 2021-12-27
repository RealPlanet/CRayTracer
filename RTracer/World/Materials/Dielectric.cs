using RTracer.Tracer;
using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;
using System;

namespace RTracer.World.Materials
{
    using Color = Vector3;

    class Dielectric : Material
    {
        public double ir; // Index of Refraction
        public Dielectric(double index_of_refraction)
        {
            ir = index_of_refraction;
        }

        private static double reflectance(double cosine, double ref_idx)
        {
            // Use Schlick's approximation for reflectance.
            double r0 = (1 - ref_idx) / (1 + ref_idx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * Math.Pow((1 - cosine), 5);
        }
        public override bool Scatter(ref Ray InputRay, ref HitInfo HitRecord, ref ScatterInfo ScatterRecord)
        {
            ScatterRecord.IsSpecular = true;
            ScatterRecord.PDF = null;
            ScatterRecord.Attenuation = new Color(1.0, 1.0, 1.0);
            double refraction_ratio = HitRecord.FrontFace ? (1.0 / ir) : ir;

            Vector3 unit_direction = InputRay.Direction.UnitVector();
            double cos_theta = Math.Min(HitRecord.Normal.Dot(-unit_direction), 1.0);
            double sin_theta = Math.Sqrt(1.0 - cos_theta * cos_theta);
            bool cannot_refract = refraction_ratio * sin_theta > 1.0;
            Vector3 direction;

            if (cannot_refract || reflectance(cos_theta, refraction_ratio) > VMath.RandomDouble())
                direction = Vector3.Reflect(unit_direction, HitRecord.Normal);
            else
                direction = Vector3.Refract(unit_direction, HitRecord.Normal, refraction_ratio);

            ScatterRecord.specular_ray = new Ray(HitRecord.Point, direction, InputRay.Time);
            return true;
        }
    }
}
