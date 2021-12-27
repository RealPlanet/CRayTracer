using RTracer.Tracer.Utility;

namespace RTracer.Tracer.Hittables
{
    class Translate : Hittable
    {
        public Hittable Obj;
        public Vector3 Offset;

        public Translate(Hittable Point, Vector3 Displacement)
        {
            Obj = Point;
            Offset = Displacement;
        }

        public override bool BoundingBox(double time0, double time1, ref AABB output_box)
        {
            if (!Obj.BoundingBox(time0, time1, ref output_box))
                return false;

            output_box = new AABB(
                output_box.Min + Offset,
                output_box.Max + Offset);

            return true;
        }

        public override bool Hit(ref Ray Ray, double t_min, double t_max, ref HitInfo HitRecord)
        {
            Ray moved_r = new Ray(Ray.Origin - Offset, Ray.Direction, Ray.Time);
            if (!Obj.Hit(ref moved_r, t_min, t_max, ref HitRecord))
                return false;

            HitRecord.Point += Offset;
            HitRecord.SetFaceNormal(moved_r, HitRecord.Normal);

            return true;
        }
    }
}
