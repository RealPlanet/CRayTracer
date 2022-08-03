
#include <iostream>
#include <fstream>
#include <sstream>

#include "Utility/VMath.h"
#include "Shapes/Box.h"
#include "BVHNode/BVHNode.h"
#include "Camera.h"
#include "Hittable/ConstantMedium.h"
#include "Hittable/HittableList.h"
#include "Materials/Material.h"
#include "Shapes/MovingSphere.h"
#include "Shapes/Sphere.h"
#include "Textures/Texture.h"
#include "Ray.h"
#include "Scenes/Scene.h"
#include "Scenes/RandomScene.h"
#include "Scenes/TwoSphereScene.h"
#include "Scenes/TwoPerlinSphereScene.h"
#include "Scenes/CornellSmokeScene.h"
#include "Scenes/ClusterScene.h"
#include "Scenes/CornellBoxScene.h"
#include "Scenes/SimpleLightScene.h"
#include "Scenes/EarthScene.h"
#include "PDF/CosinePDF.h"
#include <PDF/HittablePDF.h>
#include <PDF/MixturePDF.h>

using namespace std;


Color ray_color(const Ray& r, const Color& background, const Hittable& world, shared_ptr<Hittable> lights,int depth)
{
	HitInfo rec;

	// If we've exceeded the ray bounce limit, no more light is gathered.
	if (depth <= 0)
		return Color(0, 0, 0);

	// If the ray hits nothing, return the background color.
	if (!world.hit(r, 0.001, infinity, rec))
		return background;

	ScatterInfo srec;
    Color emitted = rec.mat_ptr->emitted(r, rec, rec.u, rec.v, rec.point);

	if (!rec.mat_ptr->scatter(r, rec, srec))
		return emitted;

	if (srec.is_specular) {
		return srec.attenuation
			* ray_color(srec.specular_ray, background, world, lights, depth - 1);
	}

	auto light_ptr = make_shared<HittablePDF>(lights, rec.point);
	MixturePDF p(light_ptr, srec.pdf_ptr);
	Ray scattered = Ray(rec.point, p.generate(), r.time());
	auto pdf_val = p.value(scattered.direction());

	return emitted
		+ srec.attenuation * rec.mat_ptr->scattering_pdf(r, rec, scattered)
		* ray_color(scattered, background, world, lights, depth - 1)
		/ pdf_val;
}

void write_color(Color pixel_color, int samples_per_pixel, std::ostream& out)
{
	double red = pixel_color.x();
	double green = pixel_color.y();
	double blu = pixel_color.z();

	// Replace NaN components with zero. See explanation in Ray Tracing: The Rest of Your Life.
	if (red != red) red = 0.0;
	if (green != green) green = 0.0;
	if (blu != blu) blu = 0.0;

	// Divide the color by the number of samples and gamma-correct for gamma=2.0.
	double scale = 1.0 / samples_per_pixel;
	red = sqrt(scale * red);
	green = sqrt(scale * green);
	blu = sqrt(scale * blu);

	// Write the translated [0,255] value of each color component.
	int f_red = static_cast<int>(256 * clamp(red, 0.0, 0.999));
	int f_green = static_cast<int>(256 * clamp(green, 0.0, 0.999));
	int f_blue = static_cast<int>(256 * clamp(blu, 0.0, 0.999));
	out << f_red << ' ' << f_green << ' ' << f_blue << '\n';
}

int main() {

    // Image
    double aspect_ratio = 16.0 / 9.0;
    int image_width = 400;
    Scene current_scene;
	shared_ptr<HittableList> lights = make_shared<HittableList>();

    switch (6)
    {
    case 1:
        current_scene = RandomScene();
        break; 
    case 2:
        current_scene = TwoSphereScene();
        break;
    case 3:
        current_scene = TwoPerlinSphereScene();
        break;
    case 4:
        current_scene = EarthScene();   
        break;
    case 5:
        current_scene = SimpleLightScene(); 
        break;
    case 6:
        current_scene = CornellBoxScene();

        aspect_ratio = 1.0;
        image_width = 600;
        break;
    case 7:
        current_scene = CornellSmokeScene();
        aspect_ratio = 1.0;
        image_width = 1024;  
        break;
    case 8:
        current_scene = ClusterScene();  
        aspect_ratio = 1.0;
        image_width = 212;   
        break;
    }

    // Camera

    Vector3 vup(0, 1, 0);
    double dist_to_focus = 10.0;
    int image_height = static_cast<int>(image_width / aspect_ratio);
    Camera scene_camera(current_scene.lookfrom, current_scene.lookat, vup, current_scene.vfov, aspect_ratio, current_scene.aperture, dist_to_focus, 0.0, 1.0);

    // Render
    ofstream image_file = {};
    image_file.open("render_image.ppm", ios::binary);
    image_file.clear();

    std::cout << "Image Width: " << image_width << "\nImage Height:" << image_height << "\n\n";
    image_file << "P3\n" << image_width << ' ' << image_height << "\n255\n";
    for (int j = image_height - 1; j >= 0; --j)
    {
        std::cerr << "\rScanlines remaining: " << j << ' ' << std::flush;

        for (int i = 0; i < image_width; ++i)
        {
            Color pixel_color(0, 0, 0);
            for (int s = 0; s < current_scene.samples_per_pixel; ++s)
            {
                //std::cout << s << std::flush;
                double u = (i + random_double()) / ((double)image_width - 1);
                double v = (j + random_double()) / ((double)image_height - 1);
                Ray r = scene_camera.get_ray(u, v);
                
                pixel_color += ray_color(r, current_scene.background, current_scene.world, current_scene.lights ,current_scene.max_depth);
            }
            write_color(pixel_color, current_scene.samples_per_pixel, image_file);
        }
    }
    image_file.close();
    std::cerr << "\nDone.\n";
}