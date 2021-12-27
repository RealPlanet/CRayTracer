using RTracer.Tracer.Utility;

namespace RTracer.Tracer
{
    abstract class PDF
    {
        public abstract double Value(Vector3 direction);
        public abstract Vector3 Generate();
    }
}
