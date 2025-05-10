using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace GraphicsDLL
{
    /// <summary>
    /// Multi-threaded iterative DeCasteljau implementation using Parallel.For
    /// </summary>
    public class IterativeParallelDeCasteljau : IterativeDeCasteljau
    {
        public IterativeParallelDeCasteljau(PointF[] controlPoints, float increment) : base(controlPoints, increment) { }

        public override PointF[] Iterate()
        {
            int numberOfSteps = (int)Math.Round(1f / increment) + 1;
            PointF[] points = new PointF[numberOfSteps];
            int chunkSize = 5;
            int numberOfChunks = (numberOfSteps + chunkSize - 1) / chunkSize;


            Parallel.For(0, numberOfChunks, chunkIndex =>
            {
                int start = chunkIndex * chunkSize;
                int end = Math.Min(start + chunkSize, numberOfSteps);
                for (int i = start; i < end; ++i)
                {
                    float t = i * increment;
                    points[i] = DecasteljauSequential(controlPoints, t);
                }
            });

            return points;
        }
    }
}