using System;
using System.Drawing;

namespace GraphicsDLL
{
    public class RecursiveSingleDeCasteljau : DeCasteljauStrategy
    {
        public RecursiveSingleDeCasteljau(PointF[] controlPoints, float increment) : base(controlPoints, increment)
        {
        }

        public override PointF[] Iterate()
        {
            int numberOfSteps = (int)Math.Round(1f / increment) + 1;
            PointF[] points = new PointF[numberOfSteps];
            
            for (int i = 0; i < numberOfSteps; ++i)
            {
                float t = i * increment;
                points[i] = DeCasteljauRecursive(
                    controlPoints,
                    t)[0]; // base condition --> will always return 1 controlPoint
            }

            return points;
        }

        private PointF[] DeCasteljauRecursive(PointF[] controlPoints, float t)
        {
            if (controlPoints.Length == 1) //base condition
                return controlPoints;

            PointF[] newPoints = new PointF[controlPoints.Length - 1];
            int i;
            for (i = 0; i < controlPoints.Length - 1; i++)
            {
                newPoints[i] = controlPoints[i].Interpolate(controlPoints[i + 1], t);
            }
            return DeCasteljauRecursive(newPoints, t);
        }
    }
}
