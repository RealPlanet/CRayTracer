#ifndef TEXTURE_H
#define TEXTURE_H

#include "Utility/VMath.h"
#include "Utility/Perlin.h"
#include <iostream>

class Texture
{
public:
    virtual Color value(double u, double v, const Color& p) const = 0;
};

class SolidColor : public Texture 
{
public:
    SolidColor() {}
    SolidColor(Color c) : color_value(c) {}
    SolidColor(double red, double green, double blue) : SolidColor(Color(red, green, blue)) {}

    virtual Color value(double u, double v, const Vector3& p) const override { return color_value; }

private:
    Color color_value;
};

class CheckerTexture : public Texture
{
public:
    CheckerTexture() {}
    CheckerTexture(shared_ptr<Texture> _even, shared_ptr<Texture> _odd) : even(_even), odd(_odd) {}

    CheckerTexture(Color c1, Color c2) : even(make_shared<SolidColor>(c1)), odd(make_shared<SolidColor>(c2)) {}

    virtual Color value(double u, double v, const Point3& p) const override {
        auto sines = sin(10 * p.x()) * sin(10 * p.y()) * sin(10 * p.z());
        if (sines < 0)
            return odd->value(u, v, p);
        else
            return even->value(u, v, p);
    }

    shared_ptr<Texture> odd;
    shared_ptr<Texture> even;
};

class NoiseTexture : public Texture
{
public:
    NoiseTexture() : scale{ 1 } {}
    NoiseTexture(double sc) : scale{ sc } {}

    virtual Color value(double u, double v, const Point3& p) const override {
        return Color(1, 1, 1) * 0.5 * (1 + sin(scale * p.z() + 10 * noise.turb(p)));
    }
    Perlin noise;
    double scale;
};

class ImageTexture : public Texture
{
public:
    const static int bytes_per_pixel = 3;

    ImageTexture() : data(nullptr), width(0), height(0), bytes_per_scanline(0) {}

    ImageTexture(const char* filename);

    ~ImageTexture() { delete data; }

    virtual Color value(double u, double v, const Vector3& p) const override;

private:
    unsigned char* data;
    int width, height;
    int bytes_per_scanline;
};

#endif