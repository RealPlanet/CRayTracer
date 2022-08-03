#ifndef PERLIN_H
#define PERLIN_H
#include "VMath.h"
class Perlin
{
public:
	Perlin();
    double noise(const Point3& p) const;
    double turb(const Point3& p, int depth = 7) const;
	~Perlin();
private:
    static const int point_count = 256;
    Vector3* ranvec;
    int* perm_x;
    int* perm_y;
    int* perm_z;
    static int* perlin_generate_perm();
    static void permute(int* p, int n);
    static double trilinear_interp(double c[2][2][2], double u, double v, double w);
    static double perlin_interp(Vector3 c[2][2][2], double u, double v, double w);
};

#endif

