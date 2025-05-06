using System;
using System.Drawing;

namespace GraphicsDLL
{
    public class RecursiveParallelDeCasteljau : DeCasteljauStrategy
    {
        public RecursiveParallelDeCasteljau(PointF[] controlPoints, float increment) : base(controlPoints, increment) { }

        public override PointF[] Iterate()
        {
            throw new NotImplementedException();
        }
    }
}