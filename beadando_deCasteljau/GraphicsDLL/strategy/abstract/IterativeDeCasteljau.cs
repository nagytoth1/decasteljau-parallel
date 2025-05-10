using System.Drawing;

namespace GraphicsDLL
{
    /// <summary>
    /// Abstract class for recursive DeCasteljau implementations
    /// </summary>
    public abstract class IterativeDeCasteljau : DeCasteljauStrategy
    {
        protected IterativeDeCasteljau(PointF[] controlPoints, float increment) : base(controlPoints, increment) { }

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
