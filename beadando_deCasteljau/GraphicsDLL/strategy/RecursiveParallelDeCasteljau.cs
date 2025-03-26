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

        protected override void DrawInternal(PointF[] controlPoints, float distance = 0.5f)
        {
            CallDrawRecursive(controlPoints, distance);
            throw new NotImplementedException("todo");
        }

        private void CallDrawRecursive(PointF[] controlPoints, float distance = 0.5f)
        {

        }
    }
}