using System;
using System.Drawing;

namespace _GraphicsDLL
{
    public static class ExtendedPointF
    {
        /// <summary>
        /// Linear interpolation between 2 given points
        /// </summary>
        /// <param name="p1">Endpoint 1</param>
        /// <param name="p2">Endpoint 2</param>
        /// <param name="distance">The distance between 2 points</param>
        /// <returns></returns>
        public static PointF Lerp(this PointF p1, PointF p2, float distance = 0.5f)
        {
            if (distance < 0 || distance > 1) throw new ArgumentException("Invalid u-value, must be between 0 and 1!");

            return new PointF(p1.X + (p2.X - p1.X) * distance,
                              p1.Y + (p2.Y - p1.Y) * distance);
        }
    }
}