#pragma once
#include "PDF.h"
#include <Utility/Vector3.h>

class MixturePDF : public PDF
{
public:
	shared_ptr<PDF> p[2];

	MixturePDF(shared_ptr<PDF> p0, shared_ptr<PDF> p1) {
		p[0] = p0;
		p[1] = p1;
	}

	virtual double value(const Vector3& direction) const override {
		return 0.5 * p[0]->value(direction) + 0.5 * p[1]->value(direction);
	}

	virtual Vector3 generate() const override
	{
		if (random_double() < 0.5)
		{
			return p[0]->generate();
		}
		else
		{
			return p[1]->generate();
		}
	}
};