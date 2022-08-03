#ifndef SPHERE_H
#define SPHERE_H

#include "Utility/VMath.h"
#include "Hittable/Hittable.h"

class Sphere : public Hittable
{
public:
    Sphere() {}
    Sphere(Point3 cen, double r, shared_ptr<Material> m) : center(cen), radius(r), mat_ptr(m){};

    virtual bool hit(const Ray& r, double t_min, double t_max, HitInfo& rec) const override;
    virtual bool bounding_box(double time0, double time1, AABB& output_box) const override;
    Vector3 random(const Point3& o) const;
    double pdf_value(const Point3& o, const Vector3& v) const;

    Point3 center;
    double radius = 0.0f;
    shared_ptr<Material> mat_ptr;
private:
    static void get_sphere_uv(const Point3& p, double& u, double& v);
};

inline Vector3 random_to_sphere(double radius, double distance_squared)
{
	auto r1 = random_double();
	auto r2 = random_double();
	auto z = 1 + r2 * (sqrt(1 - radius * radius / distance_squared) - 1);

	auto phi = 2 * pi * r1;
	auto x = cos(phi) * sqrt(1 - z * z);
	auto y = sin(phi) * sqrt(1 - z * z);

	return Vector3(x, y, z);
}

#endif // !SPHERE_H



