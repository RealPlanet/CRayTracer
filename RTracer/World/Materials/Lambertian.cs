using RTracer.Tracer;
using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;
using RTracer.World.Textures;
using System;

namespace RTracer.World.Materials
{
    using Color = Vector3;
    class Lambertian : Material
    {
        public Lambertian(Color a)
        {
            albedo = new SolidColor(a);
        }

        public Lambertian(Texture a)
        {
            albedo = a;
        }

        private Texture albedo;

        public override bool Scatter(ref Ray InputRay, ref HitInfo HitRecord, ref ScatterInfo ScatterRecord)
        {
            ScatterRecord.IsSpecular = false;
            ScatterRecord.Attenuation = albedo.Value(HitRecord.u, HitRecord.v, HitRecord.Point);
            ScatterRecord.PDF = new CosinePDF(HitRecord.Normal);
            return true;
        }

        public override double ScatteringPDF(ref Ray InputRay, ref HitInfo HitRecord, ref Ray Scattered)
        {
            var cosine = HitRecord.Normal.Dot(Scattered.Direction.UnitVector());
            return cosine < 0 ? 0 : cosine / Math.PI;
        }
    }
}
