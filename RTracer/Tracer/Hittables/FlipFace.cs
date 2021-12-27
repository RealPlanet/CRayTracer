namespace RTracer.Tracer.Hittables
{
    class FlipFace : Hittable
    {
        public Hittable HitObject;
        public FlipFace(Hittable Obj)
        {
            HitObject = Obj;
        }

        public override bool BoundingBox(double time0, double time1, ref AABB output_box)
        {
            return HitObject.BoundingBox(time0, time1, ref output_box);
        }

        public override bool Hit(ref Ray Ray, double t_min, double t_max, ref HitInfo HitRecord)
        {
            if (!HitObject.Hit(ref Ray, t_min, t_max, ref HitRecord))
                return false;

            HitRecord.FrontFace = !HitRecord.FrontFace;
            return true;
        }
    }
}
