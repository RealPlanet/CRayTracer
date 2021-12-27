using System;
using static RTracer.Tracer.Utility.VMath;

namespace RTracer.Tracer.Utility
{
    class Vector3
    {
        public static Vector3 Zero = new Vector3(0, 0, 0);

        public double[] Coordinates = new double[3] { 0, 0, 0 };
        public Vector3() { }
        public Vector3(double e0, double e1, double e2)
        {
            Coordinates[0] = e0;
            Coordinates[1] = e1;
            Coordinates[2] = e2;
        }

        public double x
        {
            get
            {
                return Coordinates[0];
            }
        }
        public double y
        {
            get
            {
                return Coordinates[1];
            }
        }
        public double z
        {
            get
            {
                return Coordinates[2];
            }
        }

        public double Dot(Vector3 Vec) => x * Vec.x + y * Vec.y + z * Vec.z;
        public Vector3 Cross(Vector3 Vec)
        {
            return new Vector3(y * Vec.z - z * Vec.y,
             z * Vec.x - x * Vec.z,
             x * Vec.y - y * Vec.x);
        }
        public Vector3 UnitVector() => this / Length();
        public double Length() => Math.Sqrt(LengthSquared());
        public double LengthSquared() => x * x + y * y + z * z;
        public bool NearZero()
        {
            const double s = 1e-8;
            return (Math.Abs(x) < s) && (Math.Abs(y) < s) && (Math.Abs(z) < s);
        }
        public static Vector3 Random()
        {
            return new Vector3(RandomDouble(), RandomDouble(), RandomDouble());
        }
        public static Vector3 Random(double Min, double Max)
        {
            return new Vector3(RandomDouble(Min, Max), RandomDouble(Min, Max), RandomDouble(Min, Max));
        }
        /// <summary>
        /// Generate a vector in a random direction from the origin within a unit sphere
        /// </summary>
        /// <returns>Randomized vector</returns>
        public static Vector3 RandomInUnitSphere()
        {
            while (true)
            {
                Vector3 PossibleResult = Random(-1, 1);

                //if (PossibleResult.LengthSquared() >= 1)
                //continue;

                return PossibleResult;
            }
        }
        public static Vector3 RandomInUnityDisk()
        {

            while (true)
            {
                Vector3 PossibleResult = new(RandomDouble(-1, 1), RandomDouble(-1, 1), 0);
                //if (PossibleResult.LengthSquared() >= 1)
                //  continue;

                return PossibleResult;
            }
        }
        /// <summary>
        /// Generate a random unit vector within a unit sphere
        /// </summary>
        /// <returns>Randomized Unit vector</returns>
        public static Vector3 RandomUnitVector() => RandomInUnitSphere().UnitVector();
        /// <summary>
        /// Generate a vector in a random direction from the origin within a unit hemisphere
        /// </summary>
        /// <param name="normal"></param>
        /// <returns>>Randomized vector</returns>
        public static Vector3 RandomInHemisphere(Vector3 normal)
        {
            Vector3 RandomizedVector = RandomInUnitSphere();

            // In the same hemisphere as the normal
            if (RandomizedVector.Dot(normal) > 0.0)
                return RandomizedVector;
            else
                return -RandomizedVector;
        }
        public static Vector3 RandomCosineDirection()
        {
            //This was double, loss of precision might be an issue
            double r1 = RandomDouble();
            double r2 = RandomDouble();
            double z = Math.Sqrt((1 - r2));

            double phi = 2 * Math.PI * r1;
            double x = Math.Cos(phi) * Math.Sqrt(r2);
            double y = Math.Sin(phi) * Math.Sqrt(r2);

            return new Vector3(x, y, z);
        }
        public static Vector3 Reflect(Vector3 v, Vector3 n) => v - 2 * v.Dot(n) * n;
        public static Vector3 Refract(Vector3 uv, Vector3 n, double etai_over_etat)
        {
            var CosTheta = Math.Min(n.Dot(-uv), 1.0d);
            Vector3 r_out_perp = etai_over_etat * (uv + CosTheta * n);
            Vector3 r_out_parallel = -Math.Sqrt(Math.Abs(1.0d - r_out_perp.LengthSquared())) * n;

            return r_out_perp + r_out_parallel;
        }
        // Operator overloads
        public static Vector3 operator -(Vector3 vec) { return new Vector3(-vec.x, -vec.y, -vec.z); }
        public static Vector3 operator -(Vector3 left, Vector3 right) { return new Vector3(left.x - right.x, left.y - right.y, left.z - right.z); }
        public static Vector3 operator /(Vector3 left, double right) { return new Vector3(left.x / right, left.y / right, left.z / right); }
        public static Vector3 operator +(Vector3 left, double right) { return new Vector3(left.x + right, left.y + right, left.z + right); }
        public static Vector3 operator +(Vector3 left, Vector3 right) { return new Vector3(left.x + right.x, left.y + right.y, left.z + right.z); }
        public static Vector3 operator *(Vector3 left, Vector3 right) { return new Vector3(left.x * right.x, left.y * right.y, left.z * right.z); }
        public static Vector3 operator *(Vector3 left, double right) { return new Vector3(left.x * right, left.y * right, left.z * right); }
        public static Vector3 operator *(double left, Vector3 right) { return new Vector3(left * right.x, left * right.y, left * right.z); }

        public double this[int i]
        {
            get { return Coordinates[i]; }
            set { Coordinates[i] = value; }
        }
    }
}
