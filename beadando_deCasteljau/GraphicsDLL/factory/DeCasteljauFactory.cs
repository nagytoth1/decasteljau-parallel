using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicsDLL
{
    public static class DeCasteljauFactory
    {
        private static readonly Dictionary<DeCasteljauStrategies, Func<PointF[], float, DeCasteljauStrategy>> algorithms = new Dictionary<DeCasteljauStrategies, Func<PointF[], float, DeCasteljauStrategy>>()
        {
            {  DeCasteljauStrategies.ITERATIVE_SINGLE_THREADED, (controlPoints, increment) => new IterativeSingleDeCasteljau(controlPoints, increment)},
            {  DeCasteljauStrategies.ITERATIVE_MULTITHREADED, (controlPoints, increment) => new IterativeParallelDeCasteljau(controlPoints, increment)},
            {  DeCasteljauStrategies.RECURSIVE_SINGLE_THREADED, (controlPoints, increment) => new RecursiveSingleDeCasteljau(controlPoints, increment)},
            {  DeCasteljauStrategies.RECURSIVE_MULTITHREADED, (controlPoints, increment) => new RecursiveParallelDeCasteljau(controlPoints, increment)},
            {  DeCasteljauStrategies.ITERATIVE_TPL_MULTITHREADED, (controlPoints, increment) => new IterativeTPLDecasteljau(controlPoints, increment)}
        };
        public static DeCasteljauStrategy Create(PointF[] controlPoints, float increment, DeCasteljauStrategies strategy)
        {
            Console.WriteLine($"Selected strategy: {strategy}");
            if(algorithms.TryGetValue(strategy, out var strategyFactory))
            {
                return strategyFactory(controlPoints, increment);
            }
            else
            {
                throw new Exception("Selected strategy does not exist in map!");
            }
        }
    }
}
