using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;

namespace RTracer.Tracer
{
    using Point3 = Vector3;

    class HittablePDF : PDF
    {
        Point3 Point;
        Hittable Hittable;
        public HittablePDF(Hittable hittable, Point3 point)
        {
            Point = point;
            Hittable = hittable;
        }

        public override Vector3 Generate() => Hittable.Random(Point);
        public override double Value(Vector3 direction) => Hittable.PDFValue(Point, direction);
    }
}
