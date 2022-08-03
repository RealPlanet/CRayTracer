#ifndef VECTOR3_H
#define VECTOR3_H
#include <cmath>
#include <iostream>
#include "VMath.h"

using std::sqrt;

class Vector3
{
public:
    Vector3() : coordinates{ 0,0,0 } {}
    Vector3(double e0, double e1, double e2) : coordinates{ e0, e1, e2 } {}

    double x() const { return coordinates[0]; }
    double y() const { return coordinates[1]; }
    double z() const { return coordinates[2]; }

    Vector3 operator-() const { return Vector3(-coordinates[0], -coordinates[1], -coordinates[2]); }
    double operator[](int i) const { return coordinates[i]; }
    double& operator[](int i) { return coordinates[i]; }

    inline double length() const { return sqrt(length_squared()); }

    inline static Vector3 random()
    {
        return Vector3(random_double(), random_double(), random_double());
    }

    inline static Vector3 random(double min, double max)
    {
        return Vector3(random_double(min, max), random_double(min, max), random_double(min, max));
    }

    double length_squared() const;
    bool near_zero() const;

    Vector3& operator+=(const Vector3& v) {
        coordinates[0] += v.coordinates[0];
        coordinates[1] += v.coordinates[1];
        coordinates[2] += v.coordinates[2];
        return *this;
    }

    Vector3& operator*=(const double t) {
        coordinates[0] *= t;
        coordinates[1] *= t;
        coordinates[2] *= t;
        return *this;
    }

    Vector3& operator/=(const double t) {
        return *this *= 1 / t;
    }

public:
    double coordinates[3];
};

// Vector3 Utility Functions

inline std::ostream& operator<<(std::ostream& out, const Vector3& v)
{
    return out << v.coordinates[0] << ' ' << v.coordinates[1] << ' ' << v.coordinates[2];
}

inline Vector3 operator+(const Vector3& u, const Vector3& v)
{
    return Vector3(u.coordinates[0] + v.coordinates[0], u.coordinates[1] + v.coordinates[1], u.coordinates[2] + v.coordinates[2]);
}

inline Vector3 operator-(const Vector3& u, const Vector3& v)
{
    return Vector3(u.coordinates[0] - v.coordinates[0], u.coordinates[1] - v.coordinates[1], u.coordinates[2] - v.coordinates[2]);
}

inline Vector3 operator*(const Vector3& u, const Vector3& v)
{
    return Vector3(u.coordinates[0] * v.coordinates[0], u.coordinates[1] * v.coordinates[1], u.coordinates[2] * v.coordinates[2]);
}

inline Vector3 operator*(double t, const Vector3& v)
{
    return Vector3(t * v.coordinates[0], t * v.coordinates[1], t * v.coordinates[2]);
}

inline Vector3 operator*(const Vector3& v, double t)
{
    return t * v;
}

inline Vector3 operator/(Vector3 v, double t)
{
    return (1 / t) * v;
}

inline double dot(const Vector3& u, const Vector3& v)
{
    return u.coordinates[0] * v.coordinates[0]
        + u.coordinates[1] * v.coordinates[1]
        + u.coordinates[2] * v.coordinates[2];
}

inline Vector3 cross(const Vector3& u, const Vector3& v)
{
    return Vector3(u.coordinates[1] * v.coordinates[2] - u.coordinates[2] * v.coordinates[1],
        u.coordinates[2] * v.coordinates[0] - u.coordinates[0] * v.coordinates[2],
        u.coordinates[0] * v.coordinates[1] - u.coordinates[1] * v.coordinates[0]);
}

inline Vector3 unit_vector(Vector3 v)
{
    return v / v.length();
}

// Generate a sphere of radius 1 then generate a Vector, if the vector is inside of the sphere return it
Vector3 random_in_unit_sphere();
Vector3 random_unit_vector();
Vector3 random_in_hemisphere(const Vector3& normal);
Vector3 reflect(const Vector3& v, const Vector3& n);
Vector3 refract(const Vector3& uv, const Vector3& n, double etai_over_etat);
Vector3 random_in_unit_disk();
Vector3 random_cosine_direction();

// Type aliases for Vector3
using Point3 = Vector3;   // 3D point
using Color = Vector3;    // RGB color

#endif