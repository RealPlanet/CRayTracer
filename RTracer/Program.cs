using System;
using System.Diagnostics;

namespace RTracer
{
    class Program
    {
        static void Main(string[] args)
        {
            Renderer SceneDrawer = new();

            Stopwatch stopWatch = new();
            stopWatch.Start();
            SceneDrawer.Start();
            stopWatch.Stop();

            Console.WriteLine($"Render took: {stopWatch.ElapsedMilliseconds} msec / {stopWatch.ElapsedMilliseconds / 1000} sec");
        }
    }
}
