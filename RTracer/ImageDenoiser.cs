using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTracer
{
    class ImageDenoiser : IDisposable
    {
        private Bitmap bitmap;
        public ImageDenoiser(string Filepath)
        {
            bitmap = LoadForFiltering(Filepath);
        }

        public Bitmap Denoise(int FilterPasses)
        {
            var Filter = new Median();
            for (int i = 0; i < FilterPasses; i++)
                Filter.ApplyInPlace(bitmap);
            return bitmap;
        }

        private static Bitmap LoadForFiltering(string filePath)
        {
            var bmp = (Bitmap)Image.FromFile(filePath);
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb)
                return bmp;

            try
            {
                // from AForge's sample code
                if (bmp.PixelFormat == PixelFormat.Format16bppGrayScale || Bitmap.GetPixelFormatSize(bmp.PixelFormat) > 32)
                    throw new NotSupportedException("Unsupported image format");

                return AForge.Imaging.Image.Clone(bmp, PixelFormat.Format24bppRgb);
            }
            finally
            {
                bmp.Dispose();
            }
        }

        public void Dispose()
        {
            bitmap.Dispose();
        }
    }
}
