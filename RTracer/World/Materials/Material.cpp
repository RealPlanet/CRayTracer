#include "Material.h"
#include <ONB/ONB.h>
#include "PDF/CosinePDF.h"

bool Lambertian::scatter(const Ray& r_in, const HitInfo& rec, ScatterInfo& srec) const
{
	srec.is_specular = false;
	srec.attenuation = albedo->value(rec.u, rec.v, rec.point);
	srec.pdf_ptr = make_shared<CosinePDF>(CosinePDF(rec.normal));
	return true;
}

double Lambertian::scattering_pdf( const Ray& r_in, const HitInfo& rec, const Ray& scattered ) const
{
    auto cosine = dot(rec.normal, unit_vector(scattered.direction()));
    return cosine < 0 ? 0 : cosine / pi;
}

bool Metal::scatter(const Ray& r_in, const HitInfo& rec, ScatterInfo& srec) const
{
	Vector3 reflected = reflect(unit_vector(r_in.direction()), rec.normal);
	srec.specular_ray = Ray(rec.point, reflected + fuzz * random_in_unit_sphere());
	srec.attenuation = m_albedo;
	srec.is_specular = true;
	srec.pdf_ptr = 0;
	return true;
}

bool Dielectric::scatter(const Ray& r_in, const HitInfo& rec, ScatterInfo& srec) const
{
	srec.is_specular = true;
	srec.pdf_ptr = nullptr;
	srec.attenuation = Color(1.0, 1.0, 1.0);
	double refraction_ratio = rec.front_face ? (1.0 / ir) : ir;

	Vector3 unit_direction = unit_vector(r_in.direction());
	double cos_theta = fmin(dot(-unit_direction, rec.normal), 1.0);
	double sin_theta = sqrt(1.0 - cos_theta * cos_theta);

	bool cannot_refract = refraction_ratio * sin_theta > 1.0;
	Vector3 direction;

	if (cannot_refract || reflectance(cos_theta, refraction_ratio) > random_double())
		direction = reflect(unit_direction, rec.normal);
	else
		direction = refract(unit_direction, rec.normal, refraction_ratio);

	srec.specular_ray = Ray(rec.point, direction, r_in.time());
	return true;
}

double Dielectric::reflectance(double cosine, double ref_idx)
{
    // Use Schlick's approximation for reflectance.
    double r0 = (1 - ref_idx) / (1 + ref_idx);
    r0 = r0 * r0;
    return r0 + (1 - r0) * pow((1 - cosine), 5);
}

bool Isotropic::scatter(const Ray& r_in, const HitInfo& rec, ScatterInfo& srec) const
{
	srec.specular_ray = Ray(rec.point, random_in_unit_sphere(), r_in.time());
	srec.attenuation = albedo->value(rec.u, rec.v, rec.point);
	return true;
}
