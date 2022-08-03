#include "HittableList.h"

bool HittableList::hit(const Ray& r, double t_min, double t_max, HitInfo& rec) const
{
    HitInfo temp_rec;
    bool hit_anything = false;
    auto closest_so_far = t_max;

    for (const auto& object : objects)
    {
        if (object->hit(r, t_min, closest_so_far, temp_rec))
        {
            hit_anything = true;
            closest_so_far = temp_rec.t;
            rec = temp_rec;
        }
    }

    return hit_anything;
}

bool HittableList::bounding_box(double time0, double time1, AABB& output_box) const
{
    if (objects.empty()) return false;

    AABB temp_box;
    bool first_box = true;

    for (const auto& object : objects)
    {
        if (!object->bounding_box(time0, time1, temp_box)) return false;
        output_box = first_box ? temp_box : surrounding_box(output_box, temp_box);
        first_box = false;
    }

    return true;
}

double HittableList::pdf_value(const Point3& o, const Vector3& v) const
{
	auto weight = 1.0 / objects.size();
	auto sum = 0.0;

	for (const auto& object : objects)
		sum += weight * object->pdf_value(o, v);

	return sum;
}

Vector3 HittableList::random(const Vector3& o) const
{
	auto int_size = static_cast<int>(objects.size());
	return objects[random_int(0, int_size - 1)]->random(o);
}