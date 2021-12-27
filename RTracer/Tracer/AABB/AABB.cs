using RTracer.Tracer.Utility;
using System;

namespace RTracer.Tracer
{
    using Point3 = Vector3;

    class AABB
    {
        private Point3 Minimum;
        private Point3 Maximum;
        public Point3 Min
        {
            get { return Minimum; }
        }
        public Point3 Max
        {
            get { return Maximum; }
        }
        public AABB()
        {
            Minimum = new Point3(0, 0, 0);
            Maximum = new Point3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        }

        public AABB(Point3 a, Point3 b) { Minimum = a; Maximum = b; }
        public bool Hit(ref Ray Ray, double t_min, double t_max)
        {
            for (int a = 0; a < 3; a++)
            {
                var t0 = Math.Min((Minimum[a] - Ray.Origin[a]) / Ray.Direction[a],
                               (Maximum[a] - Ray.Origin[a]) / Ray.Direction[a]);
                var t1 = Math.Max((Minimum[a] - Ray.Origin[a]) / Ray.Direction[a],
                               (Maximum[a] - Ray.Origin[a]) / Ray.Direction[a]);
                t_min = Math.Max(t0, t_min);
                t_max = Math.Min(t1, t_max);
                if (t_max <= t_min)
                    return false;
            }
            return true;
        }

        public static AABB SurroundingBox(AABB box0, AABB box1)
        {
            Point3 Small = new(Math.Min(box0.Min.x, box1.Min.x), Math.Min(box0.Min.y, box1.Min.y), Math.Min(box0.Min.z, box1.Min.z));

            Point3 Big = new(Math.Max(box0.Max.x, box1.Max.x), Math.Max(box0.Max.y, box1.Max.y), Math.Max(box0.Max.z, box1.Max.z));

            return new AABB(Small, Big);
        }
    }
}
