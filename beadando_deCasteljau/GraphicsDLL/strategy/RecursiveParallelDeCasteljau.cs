using GraphicsDLL;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace DeCasteljauForm
{
    public class RecursiveParallelDeCasteljau : DeCasteljauStrategy
    {
        public RecursiveParallelDeCasteljau(Graphics graphics) : base(graphics)
        {
        }

        protected override void DrawInternal(PointF[] controlPoints, float distance = 0.5F)
        {
            DrawControlPoints(controlPoints);
            byte numberOfControlPoints = (byte)controlPoints.Length;
            for (int i = 0; i < numberOfControlPoints; i++)
            {

                for (int k = 0; k < controlPoints.Length - 1; k++)
                    graphics.DrawLine(pen, controlPoints[k], controlPoints[k + 1]);

                int elementsToProcess = numberOfControlPoints - i - 1;
                if (elementsToProcess <= 0) break;

                PointF[] newControlPoints = new PointF[elementsToProcess];


                Parallel.For(0, elementsToProcess, new ParallelOptions { MaxDegreeOfParallelism = NUMBER_OF_THREADS }, j =>
                {
                    newControlPoints[j] = controlPoints[j].LinearInterpolate(controlPoints[j + 1], distance);
                });


                Array.Copy(newControlPoints, controlPoints, elementsToProcess);
            }
        }
    }
}