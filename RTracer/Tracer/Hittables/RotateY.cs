using RTracer.Tracer.Utility;
using System;

namespace RTracer.Tracer.Hittables
{
    using Point3 = Vector3;

    class RotateY : Hittable
    {
        public Hittable Obj;
        public double SinTheta;
        public double CosTheta;
        public bool HasBox;
        public AABB bbox = new();
        public RotateY(Hittable Point, double angle)
        {
            Obj = Point;
            var radians = VMath.Deg2Rad(angle);
            SinTheta = Math.Sin(radians);
            CosTheta = Math.Cos(radians);
            HasBox = Obj.BoundingBox(0, 1, ref bbox);

            Point3 min = new Point3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
            Point3 max = new Point3(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        var x = i * bbox.Max.x + (1 - i) * bbox.Min.x;
                        var y = j * bbox.Max.y + (1 - j) * bbox.Min.y;
                        var z = k * bbox.Max.z + (1 - k) * bbox.Min.z;

                        var newx = CosTheta * x + SinTheta * z;
                        var newz = -SinTheta * x + CosTheta * z;

                        Vector3 tester = new(newx, y, newz);

                        for (int c = 0; c < 3; c++)
                        {
                            min[c] = Math.Min(min[c], tester[c]);
                            max[c] = Math.Max(max[c], tester[c]);
                        }
                    }
                }
            }

            bbox = new AABB(min, max);
        }

        public override bool BoundingBox(double time0, double time1, ref AABB output_box)
        {
            output_box = bbox;
            return HasBox;
        }

        public override bool Hit(ref Ray Ray, double t_min, double t_max, ref HitInfo HitRecord)
        {
            var origin = Ray.Origin;
            var direction = Ray.Direction;

            origin[0] = CosTheta * Ray.Origin[0] - SinTheta * Ray.Origin[2];
            origin[2] = SinTheta * Ray.Origin[0] + CosTheta * Ray.Origin[2];

            direction[0] = CosTheta * Ray.Direction[0] - SinTheta * Ray.Direction[2];
            direction[2] = SinTheta * Ray.Direction[0] + CosTheta * Ray.Direction[2];

            Ray RotatedRay = new Ray(origin, direction, Ray.Time);

            if (!Obj.Hit(ref RotatedRay, t_min, t_max, ref HitRecord))
                return false;

            var Point = HitRecord.Point;
            var normal = HitRecord.Normal;

            Point[0] = CosTheta * HitRecord.Point[0] + SinTheta * HitRecord.Point[2];
            Point[2] = -SinTheta * HitRecord.Point[0] + CosTheta * HitRecord.Point[2];

            normal[0] = CosTheta * HitRecord.Normal[0] + SinTheta * HitRecord.Normal[2];
            normal[2] = -SinTheta * HitRecord.Normal[0] + CosTheta * HitRecord.Normal[2];

            HitRecord.Point = Point;
            HitRecord.SetFaceNormal(RotatedRay, normal);

            return true;
        }
    }
}
