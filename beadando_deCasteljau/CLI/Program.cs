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
            PointF[] controlPoints = FillControlPointsArray(200);
            const float increment = 0.001f;

            CompareExecutions(controlPoints, increment);
            //DeCasteljauStrategy selectedStrategy = new RecursiveParallelDeCasteljau(controlPoints, increment);
            //double sequentialTimeMs = MeasureExecutionTime(() => selectedStrategy.Iterate());

            Console.ReadKey();
        }

        private static void CompareExecutions(PointF[] controlPoints, float increment)
        {
            DeCasteljauStrategy selectedStrategy = new IterativeSingleDeCasteljau(controlPoints, increment);
            double sequentialTimeMs = MeasureExecutionTime(() => selectedStrategy.Iterate());

            selectedStrategy = new IterativeParallelDeCasteljau(controlPoints, increment);
            double parallelTimeMs1 = MeasureExecutionTime(() => selectedStrategy.Iterate());

            selectedStrategy = new IterativeTPLDecasteljau(controlPoints, increment);
            double parallelTimeMs2 = MeasureExecutionTime(() => selectedStrategy.Iterate());

            selectedStrategy = new RecursiveParallelDeCasteljau(controlPoints, increment);
            double parallelTimeMs3 = MeasureExecutionTime(() => selectedStrategy.Iterate());


            Console.WriteLine("DeCasteljau Execution Times (ms) - Average of 10 consequent executions:");
            Console.WriteLine("Number of controlPoints: {0}, increment = {1}", controlPoints.Length, increment);
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("{0,-35} {1,10}", "Strategy", "Execution Time (ms)");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("{0,-35} {1,10:F4}", "Iterative Single DeCasteljau", sequentialTimeMs);
            Console.WriteLine("{0,-35} {1,10:F4}", "Iterative Parallel DeCasteljau", parallelTimeMs1);
            Console.WriteLine("{0,-35} {1,10:F4}", "Iterative TPL DeCasteljau", parallelTimeMs2);
            Console.WriteLine("{0,-35} {1,10:F4}", "Recursive Parallel DeCasteljau", parallelTimeMs3);
            Console.WriteLine("--------------------------------------------------------");

            Console.WriteLine("Speedup with Parallel.For: {0:0.00}", sequentialTimeMs / parallelTimeMs1);
            Console.WriteLine("Speedup with TPL: {0:0.00}", sequentialTimeMs / parallelTimeMs2);
            Console.WriteLine("Speedup with Recursive + TPL: {0:0.00}", sequentialTimeMs / parallelTimeMs3);
        }

        static double MeasureExecutionTime(Func<PointF[]> method)
        {
            double totalExecutionTime = 0;
            const int NUMBER_OF_RUNS = 10;

            for (int i = 0; i < NUMBER_OF_RUNS; i++)
            {
                var sw = Stopwatch.StartNew();
                //PointF[] result = method();
                //PrintArray(result);
                method();
                sw.Stop();
                totalExecutionTime += sw.Elapsed.TotalMilliseconds;
            }

            // the average of several runs
            return totalExecutionTime / NUMBER_OF_RUNS; 
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
