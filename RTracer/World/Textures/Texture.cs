using RTracer.Tracer.Utility;

namespace RTracer.World.Textures
{
    using Color = Vector3;
    abstract class Texture
    {
        public abstract Color Value(double u, double v, Color Point);
    }
}
