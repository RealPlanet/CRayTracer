using RTracer.Tracer;
using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;

namespace RTracer.World.Materials
{
    using Color = Vector3;
    using Point3 = Vector3;

    struct ScatterInfo
    {
        public Ray specular_ray;
        public bool IsSpecular;
        public Color Attenuation;
        public PDF PDF;
    };

    class Material
    {
        public virtual bool Scatter(ref Ray InputRay, ref HitInfo HitRecord, ref ScatterInfo ScatterRecord) { return false; }
        public virtual double ScatteringPDF(ref Ray InputRay, ref HitInfo HitRecord, ref Ray Scattered) { return 0; }
        public virtual Color Emitted(ref Ray InputRay, ref HitInfo HitRecord, double u, double v, Point3 Point) { return new Color(0, 0, 0); }
    }
}
