using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace GraphicsDLL
{
    /// <summary>
    /// Abstract class that defines the DeCasteljau algorithm at a higher level for calculating Bezier curves.
    /// 
    /// This class provides a framework for different DeCasteljau implementations, allowing various strategies (e.g., sequential, parallel, TPL parallel execution) 
    /// to be applied. It holds control points and increment values and offers an abstract, derived classes only need to override the `Iterate` method for the
    /// specific iteration logic. It also includes methods for sequential interpolation of control points using the DeCasteljau algorithm.
    /// </summary>
    public abstract class DeCasteljauStrategy
    {
        protected float increment;
        protected PointF[] controlPoints;

        public float Increment {
            get { return this.increment; } 
            set {
                if (value <= 0 || value >= 1)
                {
                    throw new ArgumentException("Increment value must be greater than 0 and smaller than 1!", nameof(value));
                }
                this.increment = value;
            }
        }

        public PointF[] ControlPoints { 
            get { return this.controlPoints; } 
            set {
                if (value == null || value.Length == 0)
                {
                    throw new ArgumentException("Array of control points cannot be null or empty!", nameof(value));
                }
                this.controlPoints = value; 
            } }

        protected DeCasteljauStrategy(PointF[] controlPoints, float increment)
        {
            this.Increment = increment; 
            this.ControlPoints = controlPoints;
        }

        public abstract PointF[] Iterate();
    }
}
