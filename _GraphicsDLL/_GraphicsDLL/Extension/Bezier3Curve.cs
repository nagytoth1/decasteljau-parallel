using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace _GraphicsDLL
{
    public class Bezier3Curve
    {
        public Bezier3Curve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public Vector2 p0, p1, p2, p3;

        public static double B0(float t)
        {
            return Math.Pow(1 - t, 3);
        }
        public static double B1(float t)
        {
            return 3 * t * Math.Pow(1 - t, 2);
        }
        public static double B2(float t)
        {
            return 3 * Math.Pow(t, 2) * (1 - t);
        }
        public static double B3(float t)
        {
            return Math.Pow(t, 3);
        }

        private static Random rnd = new Random();
        private static SolidBrush pointBrush = new SolidBrush(Color.Salmon);
        private static Pen pointPen = new Pen(Color.Black, 2f);
        public static void DeCasteljau(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            int numberOfControlPoints = controlPoints.Length;
            for (int i = 0; i < numberOfControlPoints; i++)
            {
                for (int k = 0; k < controlPoints.Length - 1; k++)
                    g.DrawLineDDA(pointPen, controlPoints[k], controlPoints[k + 1]);

                for (int j = 0; j < numberOfControlPoints - i - 1; j++)
                {
                    g.DrawPoint(pointPen, pointBrush, controlPoints[j], 5f); 
                    g.DrawPoint(pointPen, pointBrush, controlPoints[numberOfControlPoints-i-1], 5f);
                    controlPoints[j] = controlPoints[j].Lerp(controlPoints[j + 1], distance);
                }
            }
            pointBrush.Color = Color.Red;
            g.DrawPoint(pointPen, pointBrush, controlPoints[0], 5f);
        }

        public static void DeCasteljauParallel(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            int numberOfControlPoints = controlPoints.Length;

            for (int i = 0; i < numberOfControlPoints; i++)
            {
                // Directly draw lines (safe since it's single-threaded)
                for (int k = 0; k < controlPoints.Length - 1; k++)
                    g.DrawLineDDA(pointPen, controlPoints[k], controlPoints[k + 1]);

                int elementsToProcess = numberOfControlPoints - i - 1;
                if (elementsToProcess <= 0) break; // No more processing needed

                PointF[] newControlPoints = new PointF[elementsToProcess]; // Buffer to store computed points

                // Parallel computation (no locking needed)
                Parallel.For(0, elementsToProcess, new ParallelOptions { MaxDegreeOfParallelism = 4 }, j =>
                {
                    newControlPoints[j] = controlPoints[j].Lerp(controlPoints[j + 1], distance);
                });

                // Draw all points after parallel computations (no locking required)
                for (int j = 0; j < elementsToProcess; j++)
                {
                    g.DrawPoint(pointPen, pointBrush, newControlPoints[j], 5f);
                }

                // Update controlPoints for the next iteration
                Array.Copy(newControlPoints, controlPoints, elementsToProcess);
            }

            // Final point in red (single-threaded, no locking needed)
            pointBrush.Color = Color.Red;
            g.DrawPoint(pointPen, pointBrush, controlPoints[0], 5f);
        }

        public static void CallDeCasteljauRecursive(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            PointF result = DeCasteljauRecursive(g, controlPoints, distance)[0];
            g.DrawPoint(new Pen(Color.Black, 2f), Brushes.Red, result, 5f);
        }

        private static PointF[] DeCasteljauRecursive(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            if (controlPoints.Length == 1) //base condition
                return controlPoints;

            PointF[] newPoints = new PointF[controlPoints.Length - 1];
            
            for (int k = 0; k < controlPoints.Length - 1; k++)
                g.DrawLineDDA(pointPen, controlPoints[k], controlPoints[k + 1]);
            int i;
            for (i = 0; i < controlPoints.Length-1; i++)
            {
                g.DrawPoint(pointPen, pointBrush, controlPoints[i], 5f);
                newPoints[i] = controlPoints[i].Lerp(controlPoints[i + 1], distance);
            }
            g.DrawPoint(pointPen, pointBrush, controlPoints[i], 5f); //draw the last point
            return DeCasteljauRecursive(g, newPoints, distance);
        }


        private static readonly object drawLock = new object(); // Lock object for thread-safe drawing

        public static void CallDeCasteljauRecursiveParallel(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            PointF[] result = DeCasteljauRecursiveParallel(g, controlPoints, distance);

            // Draw the final point in red (with thread safety)
            lock (drawLock)
            {
                g.DrawPoint(new Pen(Color.Black, 2f), Brushes.Red, result[0], 5f);
            }
        }

        private static PointF[] DeCasteljauRecursiveParallel(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            int size = controlPoints.Length;
            if (size == 1) // Base case: return the last remaining point
                return controlPoints;

            int newSize = size - 1;
            PointF[] newPoints = new PointF[newSize];

            // 🖌 Parallelized Line Drawing with Locking for Thread Safety
            Parallel.For(0, size - 1, i =>
            {
                lock (drawLock) // Ensure only one thread modifies g at a time
                {
                    g.DrawLine(new Pen(Color.Black, 1.5f), controlPoints[i], controlPoints[i + 1]);
                }
            });

            // ⚡ Parallel Lerp Computation (Thread-Safe, No Lock Needed)
            Parallel.For(0, newSize, i =>
            {
                newPoints[i] = controlPoints[i].Lerp(controlPoints[i + 1], distance);
            });

            // 🚀 Parallel Recursive Execution (Limits Thread Overhead)
            PointF[] result;
            if (newSize > 16) // Prevent excessive threading for small cases
            {
                Task<PointF[]> task = Task.Run(() => DeCasteljauRecursiveParallel(g, newPoints, distance));
                result = task.Result;
            }
            else
            {
                result = DeCasteljauRecursiveParallel(g, newPoints, distance);
            }

            return result;
        }
    }
}