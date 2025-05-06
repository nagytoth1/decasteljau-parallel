using System;
using System.Drawing;

namespace GraphicsDLL
{
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
            this.controlPoints = controlPoints;
        }

        public abstract PointF[] Iterate();

        protected PointF DecasteljauSequential(PointF[] controlPoints, float t)
        {
            PointF[] currentLevel = controlPoints;
            for (int i = 0; i < controlPoints.Length - 1; i++)
            {
                currentLevel = InterpolateControlPointsSequential(currentLevel, t);
            }

            return currentLevel[0];
        }

        protected PointF[] InterpolateControlPointsSequential(PointF[] controlPoints, float t)
        {
            int numberOfControlPoints = controlPoints.Length - 1;
            PointF[] interpolatedPoints = new PointF[numberOfControlPoints];
            for (int i = 0; i < numberOfControlPoints; i++)
            {
                interpolatedPoints[i] = controlPoints[i].Interpolate(
                    controlPoints[i + 1],
                    t);
            }
            return interpolatedPoints;
        }
    }
}
