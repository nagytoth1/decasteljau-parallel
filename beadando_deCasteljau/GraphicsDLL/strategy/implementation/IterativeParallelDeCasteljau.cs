using System;
using System.Collections;
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
            int numberOfIterations = (int)Math.Round(1f / increment); // e.g. 1 / 0.001 => 1000
            int numberOfChunks = Environment.ProcessorCount * 2;
            int chunkSize = (numberOfIterations + numberOfChunks - 1) / numberOfChunks; // dynamically calculated chunk size
            PointF[] points = new PointF[numberOfIterations];

            Parallel.For(0, numberOfChunks + 1, (index) => {
                int start = index * chunkSize;
                int end = Math.Min(start + chunkSize, numberOfIterations);
                for (int i = start; i < end; ++i)
                {
                    points[i] = DecasteljauSequential(controlPoints, i * increment);
                }
            });

            return points;
        }
    }
}