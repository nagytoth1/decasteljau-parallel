using System.Drawing;

namespace _GraphicsDLL
{
    public static class ExtendedGraphics
    {
        public static void DrawPoint(this Graphics g, Pen pen, Brush brush,
            PointF p, float r)
        {
            g.FillEllipse(brush, p.X - r, p.Y - r, 2 * r, 2 * r);
            g.DrawEllipse(pen, p.X - r, p.Y - r, 2 * r, 2 * r);
        }
    }
}
