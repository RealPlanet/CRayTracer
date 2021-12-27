using RTracer.Tracer;
using RTracer.Tracer.Utility;
using System;

namespace RTracer.World
{
    using Point3 = Vector3;

    class Camera
    {
        private Point3 origin;
        private Point3 lower_left_corner;
        private Vector3 horizontal;
        private Vector3 vertical;
        private Vector3 u, v, w;
        private double lens_radius;
        private double time0, time1;  // shutter open/close times

        public Camera(Point3 lookfrom, Point3 lookat, Vector3 vup, double vertical_fov, double aspect_ratio, double aperture, double focus_dist, double _time0, double _time1)
        {
            double theta = VMath.Deg2Rad(vertical_fov);
            double height = Math.Tan(theta / 2);
            double viewport_height = 2.0 * height;
            double viewport_width = aspect_ratio * viewport_height;

            // Camera Plane
            w = (lookfrom - lookat).UnitVector();
            u = vup.Cross(w).UnitVector();
            v = w.Cross(u);

            origin = lookfrom;
            horizontal = focus_dist * viewport_width * u;
            vertical = focus_dist * viewport_height * v;
            lower_left_corner = origin - horizontal / 2 - vertical / 2 - focus_dist * w;

            lens_radius = aperture / 2;
            time0 = _time0;
            time1 = _time1;
        }

        public Ray GetRay(double s, double t)
        {
            Vector3 rd = lens_radius * Vector3.RandomInUnityDisk();
            Vector3 offset = u * rd.x + v * rd.y;
            return new Ray(origin + offset, lower_left_corner + s * horizontal + t * vertical - origin - offset, VMath.RandomDouble(time0, time1));
        }
    }
}
