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

        private static SolidBrush pointBrush = new SolidBrush(Color.Salmon);
        private static Pen pointPen = new Pen(Color.Black, 2f);

        public static void DeCasteljau(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            int numberOfControlPoints = controlPoints.Length;
            for (int i = 0; i < numberOfControlPoints; i++)
            {
                for (int k = 0; k < controlPoints.Length - 1; k++)
                    g.DrawLine(pointPen, controlPoints[k], controlPoints[k + 1]);
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
            byte numberOfControlPoints = (byte) controlPoints.Length;
            const byte NUM_OF_THREADS = 4;
            for (int i = 0; i < numberOfControlPoints; i++)
            {
                
                for (int k = 0; k < controlPoints.Length - 1; k++)
                    g.DrawLine(pointPen, controlPoints[k], controlPoints[k + 1]);

                int elementsToProcess = numberOfControlPoints - i - 1;
                if (elementsToProcess <= 0) break; 

                PointF[] newControlPoints = new PointF[elementsToProcess]; 

                
                Parallel.For(0, elementsToProcess, new ParallelOptions { MaxDegreeOfParallelism = NUM_OF_THREADS }, j =>
                {
                    newControlPoints[j] = controlPoints[j].Lerp(controlPoints[j + 1], distance);
                });

                
                Array.Copy(newControlPoints, controlPoints, elementsToProcess);
            }
        }

        public static void CallDeCasteljauRecursive(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            PointF result = DeCasteljauRecursive(g, controlPoints, distance)[0];
            g.DrawPoint(new Pen(Color.Black, 2f), Brushes.Red, result, 5f);
        }

        private static PointF[] DeCasteljauRecursive(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            if (controlPoints.Length == 1) 
                return controlPoints;

            PointF[] newPoints = new PointF[controlPoints.Length - 1];
            
            for (int k = 0; k < controlPoints.Length - 1; k++)
                g.DrawLine(pointPen, controlPoints[k], controlPoints[k + 1]);
            int i;
            for (i = 0; i < controlPoints.Length-1; i++)
            {
                
                newPoints[i] = controlPoints[i].Lerp(controlPoints[i + 1], distance);
            }
            
            return DeCasteljauRecursive(g, newPoints, distance);
        }


        private static readonly object drawLock = new object(); 

        public static void CallDeCasteljauRecursiveParallel(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            PointF[] result = DeCasteljauRecursiveParallel(g, controlPoints, distance);

            
            lock (drawLock)
            {
                g.DrawPoint(new Pen(Color.Black, 2f), Brushes.Red, result[0], 5f);
            }
        }

        private static PointF[] DeCasteljauRecursiveParallel(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            int size = controlPoints.Length;
            if (size == 1)
                return controlPoints;

            int newSize = size - 1;
            PointF[] newPoints = new PointF[newSize];

            Parallel.For(0, size - 1, i =>
            {
                lock (drawLock) 
                {
                    g.DrawLine(new Pen(Color.Black, 1.5f), controlPoints[i], controlPoints[i + 1]);
                }
            });

            Parallel.For(0, newSize, i =>
            {
                newPoints[i] = controlPoints[i].Lerp(controlPoints[i + 1], distance);
            });

            PointF[] result;
            if (newSize > 16) 
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