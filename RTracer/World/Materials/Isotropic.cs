using RTracer.Tracer;
using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;
using RTracer.World.Textures;

namespace RTracer.World.Materials
{
    using Color = Vector3;
    class Isotropic : Material
    {
        public Isotropic(Color c)
        {
            Albedo = new SolidColor(c);
        }

        public Isotropic(Texture a)
        {
            Albedo = a;
        }

        public Texture Albedo;

        public override bool Scatter(ref Ray InputRay, ref HitInfo HitRecord, ref ScatterInfo ScatterRecord)
        {
            ScatterRecord.specular_ray = new Ray(HitRecord.Point, Vector3.RandomInUnitSphere(), InputRay.Time);
            ScatterRecord.Attenuation = Albedo.Value(HitRecord.u, HitRecord.v, HitRecord.Point);
            ScatterRecord.PDF = new CosinePDF(HitRecord.Normal);
            return true;
        }
    }
}
