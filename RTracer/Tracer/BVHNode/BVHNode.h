#ifndef BVHNODE_H
#define BVHNODE_H

#include <algorithm>
#include "Utility/VMath.h"
#include "Hittable/Hittable.h"
#include "Hittable/HittableList.h"


class BVHNode : public Hittable
{
public:
    BVHNode( HittableList& list, double time0, double time1) : BVHNode(list.objects, 0, list.objects.size(), time0, time1)
    {}
    BVHNode(std::vector<shared_ptr<Hittable>>& src_objects, size_t start, size_t end, double time0, double time1);

    virtual bool hit( const Ray& r, double t_min, double t_max, HitInfo& rec) const override;
    virtual bool bounding_box(double time0, double time1, AABB& output_box) const override;
    shared_ptr<Hittable> left;
    shared_ptr<Hittable> right;
    AABB box;
private:
};

inline bool box_compare(const shared_ptr<Hittable>& a, const shared_ptr<Hittable>& b, int axis)
{
    AABB box_a;
    AABB box_b;

    if (!a->bounding_box(0, 0, box_a) || !b->bounding_box(0, 0, box_b))
    {
        std::cerr << "No bounding box in bvh_node constructor.\n";
    }

    return box_a.min().coordinates[axis] < box_b.min().coordinates[axis];
}
inline bool box_x_compare(const shared_ptr<Hittable>& a, const shared_ptr<Hittable>& b) { return box_compare(a, b, 0); }
inline bool box_y_compare(const shared_ptr<Hittable>& a, const shared_ptr<Hittable>& b) { return box_compare(a, b, 1); }
inline bool box_z_compare(const shared_ptr<Hittable>& a, const shared_ptr<Hittable>& b) { return box_compare(a, b, 2); }

#endif

