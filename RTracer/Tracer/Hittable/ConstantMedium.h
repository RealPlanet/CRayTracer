#ifndef CONSTANT_MEDIUM_H
#define CONSTANT_MEDIUM_H

#include "Utility/VMath.h"
#include "Hittable.h"
#include "Materials/Material.h"
#include "Textures/Texture.h"

class ConstantMedium : public Hittable {
public:
    ConstantMedium(shared_ptr<Hittable> b, double d, shared_ptr<Texture> a)
        : boundary(b),
        neg_inv_density(-1 / d),
        phase_function(make_shared<Isotropic>(a))
    {}

    ConstantMedium(shared_ptr<Hittable> b, double d, Color c)
        : boundary(b),
        neg_inv_density(-1 / d),
        phase_function(make_shared<Isotropic>(c))
    {}

    virtual bool hit( const Ray& r, double t_min, double t_max, HitInfo& rec) const override;

    virtual bool bounding_box(double time0, double time1, AABB& output_box) const override {
        return boundary->bounding_box(time0, time1, output_box);
    }

public:
    shared_ptr<Hittable> boundary;
    shared_ptr<Material> phase_function;
    double neg_inv_density;
};

#endif