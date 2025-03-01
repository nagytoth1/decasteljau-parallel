using System;
using System.Drawing;

namespace _GraphicsDLL
{
    public static class ExtendedPointF
    {
        public const float GRAB_DISTANCE = 7f;

        public static bool IsGrabbedBy(this PointF p, PointF mouse)
        {
            //két pont távolsága: első koordináták 
            return  Math.Abs(p.X - mouse.X) < GRAB_DISTANCE &&
                    Math.Abs(p.Y - mouse.Y) < GRAB_DISTANCE;
        }
        public static bool IsGrabbedBy_Sqrt(this PointF p, PointF mouse)
        {
            //squareroot[ (x1 - y2) * (x1 - x2) + (y1 - y2) * (x1 - x2) ]
            return Math.Sqrt(
                (p.X - mouse.Y) * (p.X - mouse.X) +
                (p.Y - mouse.Y) * (p.X - mouse.X)) < GRAB_DISTANCE;
        }

        public static bool IsGrabbedby_Sqrt(this Vector2 vector, PointF mouse)
        {
            return ((PointF)vector).IsGrabbedBy_Sqrt(mouse);
        }
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
        public static PointF Add(this PointF p1, PointF p2)
        {
            return new PointF(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static PointF Mul(this PointF p, float a)
        {
            return new PointF(p.X * a, p.Y * a);
        }
    }
}