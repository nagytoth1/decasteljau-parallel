using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicsDLL
{
    public enum DeCasteljauStrategies
    {
        RECURSIVE_MULTITHREADED,
        ITERATIVE_MULTITHREADED,
        ITERATIVE_SINGLE_THREADED,
        RECURSIVE_SINGLE_THREADED
    }
    public static class DeCasteljauFactory
    {
        private static readonly Dictionary<DeCasteljauStrategies, Func<Graphics, DeCasteljauStrategy>> algorithms = new Dictionary<DeCasteljauStrategies, Func<Graphics, DeCasteljauStrategy>>()
        {
            {  DeCasteljauStrategies.RECURSIVE_MULTITHREADED, graphics => new RecursiveParallelDeCasteljau(graphics)},
            {  DeCasteljauStrategies.ITERATIVE_MULTITHREADED, graphics => new IterativeParallelDeCasteljau(graphics)},
            {  DeCasteljauStrategies.ITERATIVE_SINGLE_THREADED, graphics => new IterativeSingleDeCasteljau(graphics)},
            {  DeCasteljauStrategies.RECURSIVE_SINGLE_THREADED, graphics => new RecursiveSingleDeCasteljau(graphics)},
        };
        public static DeCasteljauStrategy Create(Graphics graphics, DeCasteljauStrategies strategy)
        {
            Console.WriteLine($"Selected strategy: {strategy}");
            if(algorithms.TryGetValue(strategy, out var strategyFactory))
            {
                return strategyFactory(graphics);
            }
            else
            {
                throw new Exception("Selected strategy does not exist in map!");
            }
        }
    }
}
