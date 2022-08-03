#include "Rect.h"

bool XYRect::hit(const Ray& r, double t_min, double t_max, HitInfo& rec) const
{
    double t = (k - r.origin().z()) / r.direction().z();
    if (t < t_min || t > t_max)
        return false;
    double x = r.origin().x() + t * r.direction().x();
    double y = r.origin().y() + t * r.direction().y();
    if (x < x0 || x > x1 || y < y0 || y > y1)
        return false;
    rec.u = (x - x0) / (x1 - x0);
    rec.v = (y - y0) / (y1 - y0);
    rec.t = t;
    Vector3 outward_normal = Vector3(0, 0, 1);
    rec.set_face_normal(r, outward_normal);
    rec.mat_ptr = mp;
    rec.point = r.at(t);
    return true;
}

bool YZRect::hit(const Ray& r, double t_min, double t_max, HitInfo& rec) const
{
    double t = (k - r.origin().x()) / r.direction().x();
    if (t < t_min || t > t_max)
        return false;
    double y = r.origin().y() + t * r.direction().y();
    double z = r.origin().z() + t * r.direction().z();
    if (y < y0 || y > y1 || z < z0 || z > z1)
        return false;
    rec.u = (y - y0) / (y1 - y0);
    rec.v = (z - z0) / (z1 - z0);
    rec.t = t;
    Vector3 outward_normal = Vector3(1, 0, 0);
    rec.set_face_normal(r, outward_normal);
    rec.mat_ptr = mp;
    rec.point = r.at(t);
    return true;
}

bool XZRect::hit(const Ray& r, double t_min, double t_max, HitInfo& rec) const
{
    double t = (k - r.origin().y()) / r.direction().y();
    if (t < t_min || t > t_max)
        return false;
    double x = r.origin().x() + t * r.direction().x();
    double z = r.origin().z() + t * r.direction().z();
    if (x < x0 || x > x1 || z < z0 || z > z1)
        return false;
    rec.u = (x - x0) / (x1 - x0);
    rec.v = (z - z0) / (z1 - z0);
    rec.t = t;
    Vector3 outward_normal = Vector3(0, 1, 0);
    rec.set_face_normal(r, outward_normal);
    rec.mat_ptr = mp;
    rec.point = r.at(t);
    return true;
}
