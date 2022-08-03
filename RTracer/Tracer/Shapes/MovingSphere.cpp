#include "MovingSphere.h"

Point3 MovingSphere::center(double time) const
{
    return center0 + ((time - time0) / (time1 - time0)) * (center1 - center0);
}

bool MovingSphere::hit(const Ray& r, double t_min, double t_max, HitInfo& rec) const
{
    Vector3 oc = r.origin() - center(r.time());
    double a = r.direction().length_squared();
    double half_b = dot(oc, r.direction());
    double c = oc.length_squared() - radius * radius;

    double discriminant = half_b * half_b - a * c;
    if (discriminant < 0) return false;
    double sqrtd = sqrt(discriminant);

    // Find the nearest root that lies in the acceptable range.
    double root = (-half_b - sqrtd) / a;
    if (root < t_min || t_max < root)
    {
        root = (-half_b + sqrtd) / a;
        if (root < t_min || t_max < root)
        {
            return false;
        }
    }

    rec.t = root;
    rec.point = r.at(rec.t);
    auto outward_normal = (rec.point - center(r.time())) / radius;
    rec.set_face_normal(r, outward_normal);
    rec.mat_ptr = mat_ptr;

    return true;
}

bool MovingSphere::bounding_box(double _time0, double _time1, AABB& output_box) const
{
    AABB box0( center(_time0) - Vector3(radius, radius, radius), center(_time0) + Vector3(radius, radius, radius));
    AABB box1( center(_time1) - Vector3(radius, radius, radius), center(_time1) + Vector3(radius, radius, radius));
    output_box = surrounding_box(box0, box1);
    return true;
}
