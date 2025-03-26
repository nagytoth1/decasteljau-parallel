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
    }
}
