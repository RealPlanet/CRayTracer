using RTracer.Tracer.Utility;
using System;

namespace RTracer.Tracer
{
    class CosinePDF : PDF
    {
        public CosinePDF(Vector3 w)
        {
            uvw.build_from_w(w);
        }

        public override double Value(Vector3 direction)
        {
            var cosine = direction.UnitVector().Dot(uvw.w);
            return (cosine <= 0) ? 0 : cosine / Math.PI;
        }

        public override Vector3 Generate()
        {
            return uvw.Local(Vector3.RandomCosineDirection());
        }

        private ONB uvw = new ONB();
    }
}
