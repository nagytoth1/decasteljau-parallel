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
            const int CHUNK_SIZE = 5;
            int numberOfSteps = (int) Math.Round(1f / increment) + 1;
            int numberOfChunks = (numberOfSteps + CHUNK_SIZE - 1) / CHUNK_SIZE;
            List<Task<PointF>> tasks = new List<Task<PointF>>();

            for (int chunkIndex = 0; chunkIndex < numberOfChunks; chunkIndex++)
            {

                int start = chunkIndex * CHUNK_SIZE;
                int end = Math.Min(start + CHUNK_SIZE, numberOfSteps);
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