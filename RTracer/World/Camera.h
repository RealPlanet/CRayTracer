#ifndef CAMERA_H
#define CAMERA_H

#include "Utility/VMath.h"
#include "Ray.h"

class Camera
{
public:
	Camera(Point3 lookfrom, Point3 lookat, Vector3 vup, double vertical_fov, double aspect_ratio, double aperture, double focus_dist,
		double _time0 = 0, double _time1 = 0);

	Ray get_ray(double s, double t) const;
	

private:
	Point3 origin;
	Point3 lower_left_corner;
	Vector3 horizontal;
	Vector3 vertical;
	Vector3 u, v, w;
	double lens_radius;
	double time0, time1;  // shutter open/close times
};	

#endif
