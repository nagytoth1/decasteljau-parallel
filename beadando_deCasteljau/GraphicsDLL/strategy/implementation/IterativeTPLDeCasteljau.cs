using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace GraphicsDLL
{
    /// <summary>
    /// Multi-threaded iterative DeCasteljau implementation using Task Parallel Library
    /// </summary>
    public class IterativeTPLDecasteljau : IterativeDeCasteljau
    {
        public IterativeTPLDecasteljau(PointF[] controlPoints, float increment) : base(controlPoints, increment) { }

        public override PointF[] Iterate()
        {
            int numberOfIterations = (int)Math.Round(1f / increment); // e.g. 1 / 0.001 => 1000
            int numberOfChunks = Environment.ProcessorCount * 2;
            int chunkSize = (numberOfIterations + numberOfChunks - 1) / numberOfChunks; // dynamically calculated chunk size
            List<Task<PointF>> tasks = new List<Task<PointF>>();
            for (int chunkIndex = 0; chunkIndex < numberOfChunks; ++chunkIndex)
            {
                int start = chunkIndex * chunkSize;
                int end = Math.Min(start + chunkSize, numberOfIterations);
                for (int i = start; i < end; ++i)
                {
                    float t = i * increment;
                    Task<PointF> task = Task.Run(() => DecasteljauSequential(controlPoints, t));
                    tasks.Add(task);
                }
            }

            return Task.WhenAll(tasks).Result;
        }
    }
}