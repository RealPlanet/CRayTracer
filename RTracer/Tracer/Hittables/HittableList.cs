using RTracer.Tracer.Utility;
using System;
using System.Collections.Generic;

namespace RTracer.Tracer.Hittables
{
    using Point3 = Vector3;

    class HittableList : Hittable
    {
        public List<Hittable> Objects = new List<Hittable>();

        public void Clear() => Objects.Clear();
        public void Add(Hittable obj) => Objects.Add(obj);

        public override bool Hit(ref Ray Ray, double t_min, double t_max, ref HitInfo HitRecord)
        {
            HitInfo TempRecord = new HitInfo();
            bool HasHit = false;
            var Closest = t_max;

            foreach (var obj in Objects)
            {
                if (obj.Hit(ref Ray, t_min, Closest, ref TempRecord))
                {
                    HasHit = true;
                    Closest = TempRecord.Delta;
                    HitRecord = TempRecord;
                }
            }

            return HasHit;
        }

        public override bool BoundingBox(double time0, double time1, ref AABB OutputBox)
        {
            if (Objects.Count == 0)
                return false;

            AABB TempBox = new();
            bool IsFirst = true;

            foreach (var obj in Objects)
            {
                if (!obj.BoundingBox(time0, time1, ref TempBox))
                    return false;

                OutputBox = IsFirst ? TempBox : AABB.SurroundingBox(OutputBox, TempBox);
                IsFirst = false;
            }

            return true;
        }

        public override double PDFValue(Point3 o, Vector3 v)
        {
            var weight = 1.0 / Objects.Count;
            var sum = 0.0;

            foreach (var obj in Objects)
                sum += weight * obj.PDFValue(o, v);

            return sum;
        }

        public override Vector3 Random(Vector3 o)
        {
            int size = Objects.Count;
            return Objects[new Random().Next(0, size - 1)].Random(o);
        }
    }
}
