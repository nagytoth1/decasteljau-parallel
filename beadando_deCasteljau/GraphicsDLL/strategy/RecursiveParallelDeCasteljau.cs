using GraphicsDLL;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace GraphicsDLL
{
    public class RecursiveParallelDeCasteljau : DeCasteljauStrategy
    {
        public RecursiveParallelDeCasteljau(Graphics graphics) : base(graphics)
        {
        }

        protected override PointF DrawInternal(PointF[] controlPoints, float distance = 0.5f)
        {
            return CallDrawRecursive(controlPoints, distance);
        }

        private PointF CallDrawRecursive(PointF[] controlPoints, float distance = 0.5f)
        {
            return PointF.Empty;
        }
    }
}