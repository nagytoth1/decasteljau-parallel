using System;
using System.Drawing;

namespace GraphicsDLL
{
    /// <summary>
    /// Single-threaded iterative DeCasteljau implementation
    /// </summary>
    public class RecursiveSingleDeCasteljau : RecursiveDeCasteljau
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
    }
}
