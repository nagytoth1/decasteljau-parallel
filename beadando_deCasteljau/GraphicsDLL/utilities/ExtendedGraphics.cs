using System;
using System.Drawing;

namespace GraphicsDLL
{
    public static class ExtendedGraphics
    {
        public static void DrawPoint(this Graphics graphics, Pen pen, Brush brush,
            PointF point, float radius)
        {
            graphics.DrawEllipse(pen, point.X - radius, point.Y - radius, 2 * radius, 2 * radius); // todo: is it needed?
            graphics.FillEllipse(brush, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
        }

        /// <summary>
        /// Linear interpolation between "A" and "B" points
        /// </summary>
        /// <param name="pointA">Point "A"</param>
        /// <param name="pointB">Point "B"</param>
        /// <param name="t">The distance between 2 points</param>
        /// <returns></returns>
        public static PointF Interpolate(this PointF pointA, PointF pointB, float t)
        {
            if (t < 0 || t > 1)
                throw new ArgumentException("Invalid distance value, it must be between 0 and 1.");

            return new PointF(pointA.X + (pointB.X - pointA.X) * t,
                              pointA.Y + (pointB.Y - pointA.Y) * t);
        }
    }
}
