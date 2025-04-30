using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsDLL
{
    public class RecursiveSingleDeCasteljau : DeCasteljauStrategy
    {
        public RecursiveSingleDeCasteljau(Graphics graphics) : base(graphics) { }
        protected override PointF DrawInternal(PointF[] controlPoints, float distance = 0.5F)
        {
            PointF result = DeCasteljauRecursive(graphics,
                controlPoints,
                distance)[0];
            return result;
        }

        private PointF[] DeCasteljauRecursive(Graphics graphics, PointF[] controlPoints, float distance = .5f)
        {
            if (controlPoints.Length == 1) //base condition
                return controlPoints;

            PointF[] newPoints = new PointF[controlPoints.Length - 1];
            int i;
            for (i = 0; i < controlPoints.Length - 1; i++)
            {
                newPoints[i] = controlPoints[i].LinearInterpolate(controlPoints[i + 1], distance);
            }
            return DeCasteljauRecursive(graphics,
                newPoints,
                distance);
        }
    }
}
