#pragma once
#include "PDF.h"
#include <ONB/ONB.h>
#include <Hittable/Hittable.h>

class HittablePDF :
	public PDF
{
public:
	Point3 o;
	shared_ptr<Hittable> ptr;

	HittablePDF(shared_ptr<Hittable> p, const Point3& origin) : ptr(p), o(origin) {}

	virtual double value(const Vector3& direction) const override
	{
		return ptr->pdf_value(o, direction);
	}

	virtual Vector3 generate() const override
	{
		return ptr->random(o);
	}
};