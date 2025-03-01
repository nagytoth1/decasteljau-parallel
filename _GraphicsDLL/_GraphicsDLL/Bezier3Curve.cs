using System;
using System.Drawing;
using System.Threading.Tasks;

namespace _GraphicsDLL
{
    public class Bezier3Curve
    {
        private static SolidBrush pointBrush = new SolidBrush(Color.Salmon);
        private static Pen pen = new Pen(Color.Black, 2f);
        private static readonly object drawLock = new object();

        public static void DeCasteljau(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            drawControlPoints(g, controlPoints);
            int numberOfControlPoints = controlPoints.Length;
            for (int i = 0; i < numberOfControlPoints; i++)
            {
                for (int k = 0; k < controlPoints.Length - 1; k++)
                    g.DrawLine(pen, controlPoints[k], controlPoints[k + 1]);
                for (int j = 0; j < numberOfControlPoints - i - 1; j++)
                {
                    controlPoints[j] = controlPoints[j].Lerp(controlPoints[j + 1], distance);
                }
            }
        }


        public static void DeCasteljauParallel(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            drawControlPoints(g, controlPoints);
            byte numberOfControlPoints = (byte) controlPoints.Length;
            const byte NUM_OF_THREADS = 4;
            for (int i = 0; i < numberOfControlPoints; i++)
            {
                
                for (int k = 0; k < controlPoints.Length - 1; k++)
                    g.DrawLine(pen, controlPoints[k], controlPoints[k + 1]);

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
            drawControlPoints(g, controlPoints);
        }

        private static PointF[] DeCasteljauRecursive(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            if (controlPoints.Length == 1) 
                return controlPoints;

            PointF[] newPoints = new PointF[controlPoints.Length - 1];
            
            for (int k = 0; k < controlPoints.Length - 1; k++)
                g.DrawLine(pen, controlPoints[k], controlPoints[k + 1]);
            int i;
            for (i = 0; i < controlPoints.Length-1; i++)
            {
                
                newPoints[i] = controlPoints[i].Lerp(controlPoints[i + 1], distance);
            }
            
            return DeCasteljauRecursive(g, newPoints, distance);
        }

        public static void CallDeCasteljauRecursiveParallel(Graphics g, PointF[] controlPoints, float distance = .5f)
        {
            PointF[] result = DeCasteljauRecursiveParallel(g, controlPoints, distance);
            drawControlPoints(g, controlPoints);
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
                    g.DrawLine(pen, controlPoints[i], controlPoints[i + 1]);
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

        private static void drawControlPoints(Graphics g, PointF[] controlPoints)
        {
            foreach (PointF point in controlPoints)
            {
                g.DrawPoint(pen, pointBrush, point, 5f);
            }
        }
    }
}