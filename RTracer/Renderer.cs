using RTracer.Tracer;
using RTracer.Tracer.Hittables;
using RTracer.Tracer.Utility;
using RTracer.World;
using RTracer.World.Materials;
using RTracer.World.Scenes;
using System;
using System.Drawing;
using System.Text;
using System.Threading;

namespace RTracer
{
    using Color = Vector3;

    class Renderer
    {
        private Color RayColor(ref Ray InputRay, ref Color BackgroundColor, ref HittableList WorldList, ref HittableList LightList, int Depth)
        {
            HitInfo HitRecord = new();

            // If we've exceeded the ray bounce limit, no more light is gathered.
            if (Depth <= 0)
                return new Color(0, 0, 0);

            // If the ray hits nothing, return the background color.
            if (!WorldList.Hit(ref InputRay, 0.001, double.PositiveInfinity, ref HitRecord))
                return BackgroundColor;

            ScatterInfo ScatterRecord = new();
            Color Emitted = HitRecord.Material.Emitted(ref InputRay, ref HitRecord, HitRecord.u, HitRecord.v, HitRecord.Point);

            if (!HitRecord.Material.Scatter(ref InputRay, ref HitRecord, ref ScatterRecord))
                return Emitted;

            if (ScatterRecord.IsSpecular)
            {
                return ScatterRecord.Attenuation * RayColor(ref ScatterRecord.specular_ray, ref BackgroundColor, ref WorldList, ref LightList, Depth - 1);
            }

            var LightPDF = new HittablePDF(LightList, HitRecord.Point);
            MixturePDF MixPDF = new(LightPDF, ScatterRecord.PDF);
            Ray Scattered = new(HitRecord.Point, MixPDF.Generate(), InputRay.Time);

            var pdf_val = MixPDF.Value(Scattered.Direction);

            return Emitted + ScatterRecord.Attenuation
                * HitRecord.Material.ScatteringPDF(ref InputRay, ref HitRecord, ref Scattered)
                * RayColor(ref Scattered, ref BackgroundColor, ref WorldList, ref LightList, Depth - 1)
                / pdf_val;
        }

        private static void WriteColorBitmap(Bitmap Stream, int x, int y, Color pixel_color, int samples_per_pixel)
        {
            double red = pixel_color.x;
            double green = pixel_color.y;
            double blu = pixel_color.z;

            if (double.IsNaN(red))
                red = 0;
            if (double.IsNaN(green))
                green = 0;
            if (double.IsNaN(blu))
                blu = 0;

            // Divide the color by the number of samples and gamma-correct for gamma=2.0.
            double scale = 1.0 / samples_per_pixel;
            red = Math.Sqrt(scale * red);
            green = Math.Sqrt(scale * green);
            blu = Math.Sqrt(scale * blu);

            // Write the translated [0,255] value of each color component.
            int f_red = (int)(256 * Math.Clamp(red, 0.0, 0.999));
            int f_green = (int)(256 * Math.Clamp(green, 0.0, 0.999));
            int f_blue = (int)(256 * Math.Clamp(blu, 0.0, 0.999));

            System.Drawing.Color ActualColor = System.Drawing.Color.FromArgb(f_red, f_green, f_blue);
            Stream.SetPixel(x, y, ActualColor);
        }
        public static byte[] StringToBytes(string Text)
        {
            return new UTF8Encoding(true).GetBytes(Text);
        }
        public void Start()
        {
            // Image
            double aspect_ratio = 1;
            int ImageWidth = 1080;
            Scene SceneToRender = new ExtremelyComplexScene();

            Vector3 vup = new(0, 1, 0);
            double dist_to_focus = 10.0;
            int ImageHeight = (int)(ImageWidth / aspect_ratio);
            Camera scene_camera = new(SceneToRender.lookfrom, SceneToRender.lookat, vup, SceneToRender.vfov, aspect_ratio, SceneToRender.aperture, dist_to_focus, 0.0, 1.0);
            Color[,] ImagePixels = new Color[ImageHeight, ImageWidth];

            int SquareCount = 4;
            //Assuming image sizes are even
            int SubSquareHeight = ImageHeight / SquareCount;
            int SubSquareWidth = ImageWidth / SquareCount;

            Console.WriteLine("Generating threads");
            using (var countdownEvent = new CountdownEvent(SquareCount * SquareCount))
            {
                for (int a = 0; a < SquareCount; a++)
                {
                    for (int b = 0; b < SquareCount; b++)
                    {
                        int OffsetX = (SubSquareWidth) * a;
                        int OffsetY = (SubSquareHeight) * b;
                        int samples = SceneToRender.PixelSamples;

                        ThreadPool.QueueUserWorkItem((object stateInfo) =>
                        {
                            for (int j = 0; j < SubSquareHeight; j++)
                            {
                                for (int i = 0; i < SubSquareWidth; i++)
                                {
                                    Color PixelColor = new(0, 0, 0);
                                    //Need to calculate actual pixel position within the sub image.
                                    int PixelX = i + OffsetX;
                                    int PixelY = j + OffsetY;

                                    //Console.WriteLine($"H: {heightOffsetTotal} W: {WidthOffsetTotal}\n");
                                    //Sample the pixel multiple times with a random ray
                                    for (int s = 0; s < SceneToRender.PixelSamples; ++s)
                                    {
                                        //std::cout << s << std::flush;
                                        double u = (PixelX + VMath.RandomDouble()) / ((double)ImageWidth - 1);
                                        double v = (PixelY + VMath.RandomDouble()) / ((double)ImageHeight - 1);
                                        Ray r = scene_camera.GetRay(u, v);

                                        PixelColor += RayColor(ref r, ref SceneToRender.BackgroundCol, ref SceneToRender.ObjectList, ref SceneToRender.LightList, SceneToRender.RayBounces);
                                    }

                                    ImagePixels[PixelY, PixelX] = PixelColor;
                                }
                            }
                            Console.WriteLine($"Ended Thread Work({OffsetX / SubSquareHeight},{OffsetY / SubSquareWidth})");
                            countdownEvent.Signal();
                        },
                        null);
                    }
                }
                countdownEvent.Wait();
            }

            Console.WriteLine("Threads finished work, processing image");

            using (Bitmap Map = new Bitmap(ImageWidth, ImageHeight))
            {
                Console.WriteLine($"Image Width: {ImageWidth} \nImage Height: {ImageHeight} \n\n");
                for (int i = 0; i < ImageHeight; i++)
                {
                    for (int j = 0; j < ImageWidth; j++)
                    {
                        WriteColorBitmap(Map, j, i, ImagePixels[i, j], SceneToRender.PixelSamples); //ImagePixels[i, j], , Map
                    }
                }
                Map.RotateFlip(RotateFlipType.Rotate180FlipX);
                Map.Save("render_image.png");
                Map.Dispose();

                using (var Denoiser = new ImageDenoiser("render_image.png"))
                using (Bitmap Denoised = Denoiser.Denoise(1))
                {
                    Denoised.Save("render_image.png");
                }
            }
        }
    }
}
