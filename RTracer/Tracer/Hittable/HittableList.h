#ifndef HITTABLELIST_H
#define HITTABLELIST_H

#include "Utility/VMath.h"
#include "Hittable.h"
#include <memory>
#include <vector>


using std::shared_ptr;
using std::make_shared;

class HittableList : public Hittable
{
public:
    HittableList() {}
    HittableList(shared_ptr<Hittable> object) { add(object); }

    void clear() { objects.clear(); }
    void add(shared_ptr<Hittable> object) { objects.push_back(object); }

    virtual bool hit(const Ray& r, double t_min, double t_max, HitInfo& rec) const override;
    virtual bool bounding_box( double time0, double time1, AABB& output_box) const override;
    double pdf_value(const Point3& o, const Vector3& v) const;
    Vector3 random(const Vector3& o) const;

    std::vector<shared_ptr<Hittable>> objects;
};

#endif
