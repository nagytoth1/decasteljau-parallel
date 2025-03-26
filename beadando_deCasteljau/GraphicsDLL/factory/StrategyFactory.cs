using DeCasteljauForm;
using System;
using System.Drawing;

namespace GraphicsDLL
{
    public enum DeCasteljauStrategies
    {
        ITERATIVE_SINGLETHREADED,
        ITERATIVE_MULTITHREADED,
        RECURSIVE_MULTITHREADED
    }
    public static class StrategyFactory
    {
        public static DeCasteljauStrategy Create(Graphics graphics, DeCasteljauStrategies strategy)
        {
            Console.WriteLine($"Selected strategy: {strategy}");
            if (strategy == DeCasteljauStrategies.RECURSIVE_MULTITHREADED)
                return new RecursiveParallelDeCasteljau(graphics);
            if (strategy == DeCasteljauStrategies.ITERATIVE_MULTITHREADED)
                return new IterativeParallelDeCasteljau(graphics);
            return new IterativeSingleThreadedDeCasteljau(graphics);
        }
    }
}
