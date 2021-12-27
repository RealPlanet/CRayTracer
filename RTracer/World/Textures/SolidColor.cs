using RTracer.Tracer.Utility;

namespace RTracer.World.Textures
{
    using Color = Vector3;
    class SolidColor : Texture
    {
        private Color ColorValue = new Color(0, 0, 0);
        public SolidColor() { }
        public SolidColor(Color c) { ColorValue = c; }
        public SolidColor(double red, double green, double blue) { ColorValue = new Color(red, green, blue); }

        public override Vector3 Value(double u, double v, Vector3 Point) => ColorValue;
    }
}
