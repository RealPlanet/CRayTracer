using RTracer.Tracer;
using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;
using RTracer.World.Textures;

namespace RTracer.World.Materials
{
    using Color = Vector3;
    using Point3 = Vector3;

    class DiffuseLight : Material
    {
        private Texture emit;
        public override Vector3 Emitted(ref Ray InputRay, ref HitInfo HitRecord, double u, double v, Point3 Point)
        {
            if (HitRecord.FrontFace)
            {
                return emit.Value(u, v, Point);
            }

            return new Color(0, 0, 0);
        }
        public override bool Scatter(ref Ray InputRay, ref HitInfo HitRecord, ref ScatterInfo ScatterRecord)
        {
            return false;
        }
        public DiffuseLight(Texture a)
        {
            emit = a;
        }
        public DiffuseLight(Color c)
        {
            emit = new SolidColor(c);
        }
    }
}
