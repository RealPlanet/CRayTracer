using RTracer.Tracer.Utility;

namespace RTracer.Tracer
{
    using Point3 = Vector3;
    class Ray
    {
        Point3 mOrigin;
        Vector3 mDirection;
        double mTime = 0.0f;

        public Ray() { }
        public Ray(Point3 NewOrigin, Vector3 NewDirection, double NewTime = 0.0f)
        {
            mOrigin = NewOrigin;
            mDirection = NewDirection;
            mTime = NewTime;
        }

        public Point3 Origin
        {
            get { return mOrigin; }
        }

        public Vector3 Direction
        {
            get { return mDirection; }
        }
        public double Time
        {
            get { return mTime; }
        }

        public Point3 At(double t) => Origin + t * Direction;
    }
}
