using GraphicsDLL;
using System.Drawing;

namespace DeCasteljauForm
{
    public class IterativeParallelDeCasteljau : DeCasteljauStrategy
    {
        public IterativeParallelDeCasteljau(Graphics graphics) : base(graphics)
        {
        }

        protected override void DrawInternal(PointF[] controlPoints, float distance = 0.5F)
        {
            DrawControlPoints(controlPoints);
            int numberOfControlPoints = controlPoints.Length;
            for (int i = 0; i < numberOfControlPoints; i++)
            {
                for (int k = 0; k < controlPoints.Length - 1; k++)
                    graphics.DrawLine(pen, controlPoints[k], controlPoints[k + 1]);
                for (int j = 0; j < numberOfControlPoints - i - 1; j++)
                {
                    controlPoints[j] = controlPoints[j].LinearInterpolate(controlPoints[j + 1], distance);
                }
            }
        }
    }
}