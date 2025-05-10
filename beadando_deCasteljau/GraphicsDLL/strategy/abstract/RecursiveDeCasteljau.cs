using System;
using System.Drawing;

namespace GraphicsDLL
{
    /// <summary>
    /// Abstract class for recursive DeCasteljau implementations
    /// </summary>
    public abstract class RecursiveDeCasteljau : DeCasteljauStrategy
    {
        protected RecursiveDeCasteljau(PointF[] controlPoints, float increment) : base(controlPoints, increment) { }

        protected PointF[] DeCasteljauRecursive(PointF[] points, float t)
        {
            if (points.Length == 1) //base condition
                return points;

            PointF[] newPoints = new PointF[points.Length - 1];
            for (int i = 0; i < points.Length - 1; i++)
            {
                newPoints[i] = points[i].Interpolate(points[i + 1], t);
            }
            return DeCasteljauRecursive(newPoints, t);
        }
    }
}