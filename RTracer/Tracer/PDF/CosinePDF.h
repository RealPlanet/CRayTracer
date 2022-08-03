#pragma once
#include "PDF.h"
#include <ONB/ONB.h>

class CosinePDF :
    public PDF
{
public:
	CosinePDF(const Vector3& w) { uvw.build_from_w(w); }

	virtual double value(const Vector3& direction) const override
	{
		auto cosine = dot(unit_vector(direction), uvw.w());
		return (cosine <= 0) ? 0 : cosine / pi;
	}

	virtual Vector3 generate() const override
	{
		return uvw.local(random_cosine_direction());
	}

public:
	ONB uvw;
};

