using GraphicsDLL;
using System.Drawing;

namespace GraphicsDLL
{
    public class IterativeParallelDeCasteljau : DeCasteljauStrategy
    {
        public IterativeParallelDeCasteljau(Graphics graphics) : base(graphics)
        {
        }

        protected override PointF DrawInternal(PointF[] controlPoints, float distance = 0.5F)
        {
            int numberOfControlPoints = controlPoints.Length;
            for (int i = 0; i < numberOfControlPoints; i++)
            {
                for (int j = 0; j < numberOfControlPoints - i - 1; j++)
                {
                    controlPoints[j] = controlPoints[j].LinearInterpolate(controlPoints[j + 1], distance);
                }
            }
            return controlPoints[0];
        }
    }
}