using RTracer.Tracer.Hittables;
using System;
using System.Collections.Generic;

namespace RTracer.Tracer
{
    class BVHNode : Hittable
    {
        public Hittable left;
        public Hittable right;
        public AABB box = new();

        public BVHNode(HittableList list, double time0, double time1) : this(list.Objects, 0, list.Objects.Count, time0, time1) { }

        public BVHNode(List<Hittable> src_objects, int start, int end, double time0, double time1)
        {
            List<Hittable> objects = src_objects; // Create a modifiable array of the source scene object
            int axis = new Random().Next(0, 2);

            //Pick which comparator we need based on axis
            Func<Hittable, Hittable, int> comparator = (axis == 0) ? BoxXCompare : (axis == 1) ? BoxYCompare : BoxZCompare;
            int object_span = end - start;

            if (object_span == 1)
            {
                left = right = objects[start];
            }
            else if (object_span == 2)
            {
                //objects[start], objects[start + 1]
                if (comparator(objects[start], objects[start + 1]) == 0)
                {
                    left = objects[start];
                    right = objects[start + 1];
                }
                else
                {
                    left = objects[start + 1];
                    right = objects[start];
                }
            }
            else
            {
                objects.Sort((x, y) => comparator(x, y));

                var mid = start + object_span / 2;
                left = new BVHNode(objects, start, mid, time0, time1);
                right = new BVHNode(objects, mid, end, time0, time1);

                AABB box_left = new();
                AABB box_right = new();
                if (!left.BoundingBox(time0, time1, ref box_left) || !right.BoundingBox(time0, time1, ref box_right))
                {
                    Console.WriteLine("No bounding box in bvh_node constructor.\n");
                }


                box = AABB.SurroundingBox(box_left, box_right);
            }
        }

        public override bool BoundingBox(double time0, double time1, ref AABB output_box)
        {
            output_box = box;
            return true;
        }

        public override bool Hit(ref Ray Ray, double t_min, double t_max, ref HitInfo HitRecord)
        {
            if (!box.Hit(ref Ray, t_min, t_max))
            {
                return false;
            }

            bool hit_left = left.Hit(ref Ray, t_min, t_max, ref HitRecord);
            bool hit_right = right.Hit(ref Ray, t_min, hit_left ? HitRecord.Delta : t_max, ref HitRecord);

            return hit_left || hit_right;
        }

        public static int BoxCompare(Hittable a, Hittable b, int axis)
        {
            AABB box_a = new();
            AABB box_b = new();

            if (!a.BoundingBox(0, 0, ref box_a) || !b.BoundingBox(0, 0, ref box_b))
            {
                Console.WriteLine("No bounding box in bvh_node constructor.\n");
            }

            return (int)(box_a.Max.Coordinates[axis] - box_b.Min.Coordinates[axis]);
        }

        public static int BoxXCompare(Hittable a, Hittable b) { return BoxCompare(a, b, 0); }
        public static int BoxYCompare(Hittable a, Hittable b) { return BoxCompare(a, b, 1); }
        public static int BoxZCompare(Hittable a, Hittable b) { return BoxCompare(a, b, 2); }
    }
}
