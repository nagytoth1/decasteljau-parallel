using System;
using System.Diagnostics;
using System.Drawing;
using GraphicsDLL;

namespace CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PointF[] controlPoints = FillControlPointsArray(100);
            const float increment = 0.001f;
            DeCasteljauStrategy selectedStrategy = new IterativeSingleDeCasteljau(controlPoints, increment);
            double avgSequentialTimeMs = MeasureAverageRuntime(() => selectedStrategy.Iterate());
            selectedStrategy = new IterativeParallelDeCasteljau(controlPoints, increment);
            double avgParallelTimeMs = MeasureAverageRuntime(() => selectedStrategy.Iterate());
            Console.WriteLine("Szekvenciális DeCasteljau: végrehajtási idő {0} ms", avgSequentialTimeMs);
            Console.WriteLine("Párhuzamos DeCasteljau: végrehajtási idő {0} ms", avgParallelTimeMs);
            Console.WriteLine("Gyorsítás: {0:0.00}", avgSequentialTimeMs / avgParallelTimeMs);
            Console.ReadKey();
        }

        static double MeasureAverageRuntime(Func<PointF[]> method, int numberOfRuns = 10)
        {
            double totalRunTime = 0;
            for (int i = 0; i < numberOfRuns; i++)
            {
                var sw = Stopwatch.StartNew();
                //PointF[] result = method();
                //PrintArray(result);
                method();
                sw.Stop();
                Console.WriteLine($"végrehajtási idő = {sw.Elapsed.TotalMilliseconds} ms");
                totalRunTime += sw.Elapsed.TotalMilliseconds;
            }
            return totalRunTime / numberOfRuns;
        }

        private static PointF[] FillControlPointsArray(int numberOfControlPoints)
        {
            PointF[] generatedControlPoints = new PointF[numberOfControlPoints];

            for (int i = 0; i < numberOfControlPoints; i++)
            {
                generatedControlPoints[i] = new PointF(i, i);
            }

            return generatedControlPoints;
        }

        private static void PrintArray(PointF[] points)
        {
            Console.Write("[");
            foreach (PointF point in points)
            {
                Console.Write($" {point} ");
            }
            Console.WriteLine("]");
        }
    }
}
