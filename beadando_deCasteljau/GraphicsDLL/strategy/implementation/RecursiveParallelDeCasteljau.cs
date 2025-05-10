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
            int numberOfSteps = (int)Math.Round(1f / increment) + 1;
            List<Task<PointF>> tasks = new List<Task<PointF>>();

            for (int i = 0; i < numberOfSteps; ++i)
            {
                float t = i * increment;
                Task<PointF> task = Task.Run(() =>
                {
                    return DeCasteljauRecursive(
                    controlPoints,
                    t)[0]; // base condition --> will always return 1 controlPoint
                });
                tasks.Add(task);
            }

            // block until all tasks are completed to collect results
            return Task.WhenAll(tasks.ToArray()).Result;
        }
    }
}