using System;
using System.Drawing;

namespace GraphicsDLL
{
    public static class ExtendedPointF
    {
        /// <summary>
        /// Linear interpolation between "A" and "B" points
        /// </summary>
        /// <param name="pointA">Point "A"</param>
        /// <param name="pointB">Point "B"</param>
        /// <param name="distance">The distance between 2 points</param>
        /// <returns></returns>
        public static PointF LinearInterpolate(this PointF pointA, PointF pointB, float distance = 0.5f)
        {
            if (distance < 0 || distance > 1) throw new ArgumentException("Invalid distance value, it must be between 0 and 1.");

            return new PointF(pointA.X + (pointB.X - pointA.X) * distance,
                              pointA.Y + (pointB.Y - pointA.Y) * distance);
        }
    }
}