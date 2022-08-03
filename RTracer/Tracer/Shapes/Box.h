#ifndef BOX_H
#define BOX_H

#include "Utility/VMath.h"
#include "Rect.h"
#include "Hittable/HittableList.h"

class Box : public Hittable
{
public:
	Box() {}
	Box(const Point3& p0, const Point3& p1, shared_ptr<Material> ptr);
    virtual bool hit(const Ray& r, double t_min, double t_max, HitInfo& rec) const override;
    virtual bool bounding_box(double time0, double time1, AABB& output_box) const override
    {
        output_box = AABB(box_min, box_max);
        return true;
    }

public:
    Point3 box_min;
    Point3 box_max;
    HittableList sides;
};

#endif
