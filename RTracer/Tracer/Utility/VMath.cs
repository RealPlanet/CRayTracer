using RTracer.Tracer.Hittables;
using System;
using System.Collections.Generic;

namespace RTracer.Tracer.Utility
{
    static class VMath
    {
        public static double RandomDouble(double Min, double Max)
        {
            Random random = new Random();
            double val = (random.NextDouble() * (Max - Min) + Min);
            return val;
        }

        public static double RandomDouble()
        {
            Random random = new Random();
            return random.NextDouble();
        }

        public static double Deg2Rad(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
