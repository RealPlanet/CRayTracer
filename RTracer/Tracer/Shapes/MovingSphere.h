
#ifndef MOVING_SPHERE_H
#define MOVING_SPHERE_H

#include "Utility/VMath.h"
#include "Hittable/Hittable.h"

class MovingSphere : public Hittable
{
public:
    MovingSphere() {}
    MovingSphere( Point3 cen0, Point3 cen1, double _time0, double _time1, double r, shared_ptr<Material> m)
                : center0(cen0), center1(cen1), time0(_time0), time1(_time1), radius(r), mat_ptr(m) {}

    virtual bool hit( const Ray& r, double t_min, double t_max, HitInfo& rec ) const override;
    virtual bool bounding_box( double _time0, double _time1, AABB& output_box) const override;
    Point3 center(double time) const;

    Point3 center0, center1;
    double time0 = 0.0f;
    double time1 = 0.0f;
    double radius = 0.0f;
    shared_ptr<Material> mat_ptr;
};

#endif
