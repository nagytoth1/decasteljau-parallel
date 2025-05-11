using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace GraphicsDLL
{
    /// <summary>
    /// Multi-threaded recursive DeCasteljau implementation using Task Parallel Library
    /// </summary>
    public class RecursiveParallelDeCasteljau : RecursiveDeCasteljau
    {
        public RecursiveParallelDeCasteljau(PointF[] controlPoints, float increment) : base(controlPoints, increment) { }

        public override PointF[] Iterate()
        {
            int numberOfIterations = (int)Math.Round(1f / increment); // e.g. 1 / 0.001 => 1000
            int numberOfChunks = Environment.ProcessorCount * 2;
            int chunkSize = (numberOfIterations + numberOfChunks - 1) / numberOfChunks; // dynamically calculated chunk size
            List<Task<PointF[]>> tasks = new List<Task<PointF[]>>();

            for (int chunkIndex = 0; chunkIndex < numberOfChunks; ++chunkIndex)
            {
                int start = chunkIndex * chunkSize;
                int end = Math.Min(start + chunkSize, numberOfIterations);
                Task<PointF[]> task = Task.Run(() =>
                {
                    PointF[] localResults = new PointF[end - start];
                    for (int i = start; i < end; ++i)
                    {
                        localResults[i - start] = DeCasteljauRecursive(controlPoints, i * increment)[0];
                    }
                    return localResults;
                });
                tasks.Add(task);
            }

            // merge all results into this array
            PointF[] globalResults = new PointF[numberOfIterations];
            int k = 0;
            for (int i = 0; i < numberOfChunks; i++)
            {
                PointF[] taskResults = tasks[i].Result; // we could block here, right?
                foreach (var resultPoint in taskResults)
                {
                    globalResults[k++] = resultPoint;
                }
            }
            // block until all tasks are completed to collect results
            return globalResults;
        }
    }
}