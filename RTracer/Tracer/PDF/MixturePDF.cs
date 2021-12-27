using RTracer.Tracer.Utility;
using static RTracer.Tracer.Utility.VMath;

namespace RTracer.Tracer
{
    class MixturePDF : PDF
    {
        public PDF[] PDF = new PDF[2];
        public MixturePDF(PDF p0, PDF p1)
        {
            PDF[0] = p0;
            PDF[1] = p1;
        }
        public override Vector3 Generate()
        {
            if (RandomDouble() < 0.5)
            {
                return PDF[0].Generate();
            }
            else
            {
                return PDF[1].Generate();
            }
        }
        public override double Value(Vector3 direction)
        {
            double firstDir = PDF[0].Value(direction);
            double secondDir = PDF[1].Value(direction);
            double val = 0.5 * firstDir + 0.5 * secondDir;
            return val;
        }
    }
}
