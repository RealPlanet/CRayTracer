#ifndef AARECT_H
#define AARECT_H

#include "Utility/VMath.h"
#include "Hittable/Hittable.h"

class XYRect : public Hittable
{
public:
    XYRect() {}
    XYRect(double _x0, double _x1, double _y0, double _y1, double _k, shared_ptr<Material> mat)
        : x0{ _x0 }, x1{ _x1 }, y0{ _y0 }, y1{_y1}, k{_k}, mp{ mat } {}

    virtual bool hit(const Ray& r, double t_min, double t_max, HitInfo& rec) const override;
    virtual bool bounding_box(double time0, double time1, AABB& output_box) const override {
        // The bounding box must have non-zero width in each dimension, so pad the Z
        // dimension a small amount.
        output_box = AABB(Point3(x0, y0, k - 0.0001), Point3(x1, y1, k + 0.0001));
        return true;
    }

    shared_ptr<Material> mp;
    double x0 = 0.0f;
    double x1 = 0.0f;
    double y0 = 0.0f;
    double y1 = 0.0f;
    double k = 0.0f;
};

class XZRect : public Hittable
{
public:
    XZRect() {}

    XZRect(double _x0, double _x1, double _z0, double _z1, double _k, shared_ptr<Material> mat)
        : x0{ _x0 }, x1{ _x1 }, z0{ _z0 }, z1{ _z1 }, k{ _k }, mp{ mat } {}

    virtual bool hit(const Ray& r, double t_min, double t_max, HitInfo& rec) const override;

    virtual bool bounding_box(double time0, double time1, AABB& output_box) const override {
        // The bounding box must have non-zero width in each dimension, so pad the Y
        // dimension a small amount.
        output_box = AABB(Point3(x0, k - 0.0001, z0), Point3(x1, k + 0.0001, z1));
        return true;
    }

	virtual double pdf_value(const Point3& origin, const Vector3& v) const override
    {
        HitInfo rec;
		if (!this->hit(Ray(origin, v), 0.001, infinity, rec))
			return 0;

		auto area = (x1 - x0) * (z1 - z0);
		auto distance_squared = rec.t * rec.t * v.length_squared();
		auto cosine = fabs(dot(v, rec.normal) / v.length());

		return distance_squared / (cosine * area);
	}

	virtual Vector3 random(const Point3& origin) const override
    {
		auto random_point = Point3(random_double(x0, x1), k, random_double(z0, z1));
		return random_point - origin;
	}

    shared_ptr<Material> mp;
    double x0 = 0.0f;
    double x1 = 0.0f;
    double z0 = 0.0f;
    double z1 = 0.0f;
    double k = 0.0f;
};

class YZRect : public Hittable
{
public:
    YZRect() {}

    YZRect(double _y0, double _y1, double _z0, double _z1, double _k,
        shared_ptr<Material> mat)
        : y0(_y0), y1(_y1), z0(_z0), z1(_z1), k(_k), mp(mat) {};

    virtual bool hit(const Ray& r, double t_min, double t_max, HitInfo& rec) const override;

    virtual bool bounding_box(double time0, double time1, AABB& output_box) const override {
        // The bounding box must have non-zero width in each dimension, so pad the X
        // dimension a small amount.
        output_box = AABB(Point3(k - 0.0001, y0, z0), Point3(k + 0.0001, y1, z1));
        return true;
    }
    shared_ptr<Material> mp;
    double z0 = 0.0f;
    double z1 = 0.0f;
    double y0 = 0.0f;
    double y1 = 0.0f;
    double k = 0.0f;
};

#endif