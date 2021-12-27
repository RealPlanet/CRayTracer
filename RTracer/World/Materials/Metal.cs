using RTracer.Tracer;
using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;

namespace RTracer.World.Materials
{
    using Color = Vector3;

    class Metal : Material
    {
        Color MatAlbedo;
        double Fuzz;

        public Metal(Color a, double f)
        {
            MatAlbedo = a;
            Fuzz = f < 1 ? f : 1;
        }

        public override bool Scatter(ref Ray InputRay, ref HitInfo HitRecord, ref ScatterInfo ScatterRecord)
        {
            Vector3 reflected = Vector3.Reflect(InputRay.Direction.UnitVector(), HitRecord.Normal);
            ScatterRecord.specular_ray = new Ray(HitRecord.Point, reflected + Fuzz * Vector3.RandomInUnitSphere(), InputRay.Time);
            ScatterRecord.Attenuation = MatAlbedo;
            ScatterRecord.IsSpecular = true;
            ScatterRecord.PDF = null;
            return true;
        }
    }
}
