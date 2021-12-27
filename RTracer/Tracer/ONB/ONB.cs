using RTracer.Tracer.Utility;
using System;

namespace RTracer.Tracer
{
    /// <summary>
    /// Orthonormal Basis
    /// </summary>
    class ONB
    {
        public Vector3[] Axis = new Vector3[3];
        public Vector3 this[int i]
        {
            get { return Axis[i]; }
            set { Axis[i] = value; }
        }

        public Vector3 u { get { return this[0]; } }
        public Vector3 v { get { return this[1]; } }
        public Vector3 w { get { return this[2]; } }

        public Vector3 Local(double a, double b, double c) => a * u + b * v + c * w;

        public Vector3 Local(Vector3 a)
        {
            return a.x * u + a.y * v + a.z * w;
        }

        public void build_from_w(Vector3 n)
        {
            this[2] = n.UnitVector();
            Vector3 a = (Math.Abs(w.x) > 0.9) ? new Vector3(0, 1, 0) : new Vector3(1, 0, 0);
            this[1] = w.Cross(a).UnitVector();
            this[0] = w.Cross(v);
        }
    }
}
