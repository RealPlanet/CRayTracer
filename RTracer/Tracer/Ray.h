#ifndef RAY_H
#define RAY_H

#include "Utility/Vector3.h"

class Ray
{
public:
	
	Ray() {}
	Ray(const Point3& _origin, const Vector3& _direction, double _time = 0.0f) : m_origin{ _origin }, m_direction{ _direction }, m_time{ _time }
	{}

	Point3 origin() const { return m_origin; }
	Vector3 direction() const { return m_direction; }
	double time() const { return m_time; }

	Point3 at(double t) const
	{
		return m_origin + t * m_direction;
	}

	Point3 m_origin;
	Vector3 m_direction;
	double m_time = 0.0f;
};

#endif // !RAY_H