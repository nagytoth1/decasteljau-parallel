using GraphicsDLL;
using System;
using System.Drawing;

namespace GraphicsDLL
{
    public class IterativeSingleDeCasteljau : DeCasteljauStrategy
    {
        public IterativeSingleDeCasteljau(PointF[] controlPoints, float increment) : base(controlPoints, increment) { }

        public override PointF[] Iterate()
        {
            int numberOfSteps = (int)Math.Round(1f / increment) + 1;

            PointF[] points = new PointF[numberOfSteps];
            for (int i = 0; i < numberOfSteps; i++)
            {
                float t = i * increment;
                points[i] = DecasteljauSequential(controlPoints, t);
            }
            return points;
        }
    }
}