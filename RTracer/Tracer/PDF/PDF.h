#ifndef PDF_H
#define PDF_H
class PDF {
public:
	virtual ~PDF() {}

	virtual double value(const Vector3& direction) const = 0;
	virtual Vector3 generate() const = 0;
};

#endif