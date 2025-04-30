using GraphicsDLL;
using System.Drawing;

namespace GraphicsDLL
{
    public class IterativeSingleDeCasteljau : DeCasteljauStrategy
    {
        public const int DECASTELJAU_DISTANCE_MAX = 1;

        public IterativeSingleDeCasteljau(Graphics graphics) : base(graphics)
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