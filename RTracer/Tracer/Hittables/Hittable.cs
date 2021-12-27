using RTracer.Tracer.Utility;
using RTracer.World.Materials;

namespace RTracer.Tracer.Hittables
{
    using Point3 = Vector3;
    struct HitInfo
    {
        public Point3 Point;
        public Vector3 Normal;
        public Material Material;
        public double Delta;
        public bool FrontFace;
        // Surface coordinates
        public double u;
        public double v;

        public void SetFaceNormal(Ray Ray, Point3 outward_normal)
        {
            FrontFace = Ray.Direction.Dot(outward_normal) < 0;
            Normal = FrontFace ? outward_normal : -outward_normal;
        }
    }

    /*
	*	Abstract class which is derived for all objects that can be hit with a ray
	*/
    abstract class Hittable
    {
        /*
			This function is implemented on derived objects and calculates the hit behaviour for the ray
			output in HitRecord
		*/
        public abstract bool Hit(ref Ray Ray, double TimeMin, double TimeMax, ref HitInfo HitRecord);
        public abstract bool BoundingBox(double Time0, double Time1, ref AABB OutputBox);
        public virtual double PDFValue(Point3 o, Vector3 v) { return 0.0; }
        public virtual Vector3 Random(Vector3 o) { return new Vector3(1, 0, 0); }
    }
}
