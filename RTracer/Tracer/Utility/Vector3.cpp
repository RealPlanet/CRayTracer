#include "Vector3.h"

double Vector3::length_squared() const
{
	return coordinates[0] * coordinates[0] + coordinates[1] * coordinates[1] + coordinates[2] * coordinates[2];
}

bool Vector3::near_zero() const
{
    // Return true if the vector is close to zero in all dimensions.
    const auto s = 1e-8;
    return (fabs(coordinates[0]) < s) && (fabs(coordinates[1]) < s) && (fabs(coordinates[2]) < s);
}

Vector3 random_in_unit_sphere()
{
    while (true)
    {
        Vector3 p = Vector3::random(-1, 1);
        if (p.length_squared() >= 1) continue;
        return p;
    }
}

Vector3 random_unit_vector()
{
    return unit_vector(random_in_unit_sphere());
}

Vector3 random_in_hemisphere(const Vector3& normal)
{
    Vector3 in_unit_sphere = random_in_unit_sphere();

    if (dot(in_unit_sphere, normal) > 0.0) // In the same hemisphere as the normal
        return in_unit_sphere;
    else
        return -in_unit_sphere;
}

Vector3 reflect(const Vector3& v, const Vector3& n)
{
    return v - 2 * dot(v, n) * n;
}

Vector3 refract(const Vector3& uv, const Vector3& n, double etai_over_etat)
{
    auto cos_theta = fmin(dot(-uv, n), 1.0);
    Vector3 r_out_perp = etai_over_etat * (uv + cos_theta * n);
    Vector3 r_out_parallel = -sqrt(fabs(1.0 - r_out_perp.length_squared())) * n;
    return r_out_perp + r_out_parallel;
}

Vector3 random_in_unit_disk()
{
    while (true)
    {
        Vector3 p = Vector3(random_double(-1, 1), random_double(-1, 1), 0);
        if (p.length_squared() >= 1) continue;
        return p;
    }
}

Vector3 random_cosine_direction()
{
	double r1 = random_double();
	double r2 = random_double();
	double z = sqrt(1 - r2);

	double phi = 2 * pi * r1;
	double x = cos(phi) * sqrt(r2);
	double y = sin(phi) * sqrt(r2);

	return Vector3(x, y, z);
}
