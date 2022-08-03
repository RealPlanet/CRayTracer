#ifndef MATERIAL_H
#define MATERIAL_H

#include "Utility/VMath.h"

#include "Hittable/Hittable.h"
#include "Textures/Texture.h"
#include "PDF/PDF.h"

struct ScatterInfo
{
	Ray specular_ray;
	bool is_specular;
	Color attenuation;
	shared_ptr<PDF> pdf_ptr;
};

class Material
{
public:
    virtual bool scatter(const Ray& r_in, const HitInfo& rec, ScatterInfo& srec) const
    {
        return false;
    };

    virtual double scattering_pdf(const Ray& r_in, const HitInfo& rec, const Ray& scattered) const
    {
        return 0;
    }

	virtual Color emitted(const Ray& r_in, const HitInfo& rec, double u, double v, const Point3& p) const { return Color(0, 0, 0);
	}
};

class Lambertian : public Material
{
public:
    Lambertian(const Color& a) : albedo(make_shared<SolidColor>(a)) {}
    Lambertian(shared_ptr<Texture> a) : albedo(a) {}
    // Inherited via Material
    virtual bool scatter(const Ray& r_in, const HitInfo& rec, ScatterInfo& srec) const override;
    virtual double scattering_pdf(const Ray& r_in, const HitInfo& rec, const Ray& scattered) const override;

    Color m_albedo;
    shared_ptr<Texture> albedo;
};

class Metal : public Material
{
public:
    Metal(const Color& a, double f) : m_albedo(a), fuzz(f < 1 ? f : 1) {}

    virtual bool scatter(const Ray& r_in, const HitInfo& rec, ScatterInfo& srec) const override;

public:
    Color m_albedo;
    double fuzz;
};

class Dielectric : public Material
{
public:
    Dielectric(double index_of_refraction) : ir(index_of_refraction) {}

    virtual bool scatter(const Ray& r_in, const HitInfo& rec, ScatterInfo& srec) const override;
    double ir; // Index of Refraction

private:
    static double reflectance(double cosine, double ref_idx);
};

class DiffuseLight : public Material
{
public:
    DiffuseLight(shared_ptr<Texture> a) : emit(a) {}
    DiffuseLight(Color c) : emit(make_shared<SolidColor>(c)) {}

    virtual bool scatter(const Ray& r_in, const HitInfo& rec, ScatterInfo& srec) const override { return false; }
    virtual Color emitted(const Ray& r_in, const HitInfo& rec, double u, double v, const Point3& p) const override
    { 
        if (rec.front_face)
        {
            return emit->value(u, v, p);
        }

		return Color(0, 0, 0);
    }
    shared_ptr<Texture> emit;
};

class Isotropic : public Material {
public:
    Isotropic(Color c) : albedo(make_shared<SolidColor>(c)) {}
    Isotropic(shared_ptr<Texture> a) : albedo(a) {}

    virtual bool scatter(const Ray& r_in, const HitInfo& rec, ScatterInfo& srec) const override;

public:
    shared_ptr<Texture> albedo;
};

#endif
