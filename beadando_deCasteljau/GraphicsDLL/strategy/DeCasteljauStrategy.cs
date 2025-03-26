using System;
using System.Drawing;

namespace GraphicsDLL
{
    public abstract class DeCasteljauStrategy
    {
        protected const float POINT_SIZE = 3.5f;
        protected const byte NUMBER_OF_THREADS = 4;

        protected readonly SolidBrush brush;
        protected readonly Pen pen;
        protected readonly object drawLock;

        protected Graphics graphics;

        protected DeCasteljauStrategy(Graphics graphics)
        {
            this.graphics = graphics;
            brush = new SolidBrush(Color.Salmon);
            pen = new Pen(Color.Black, 2f);
            drawLock = new object();
        }

        public void Draw(PointF[] controlPoints, float distance = .5f){
            if (controlPoints == null || controlPoints.Length == 0) throw new ArgumentException("Control points should not be null or empty!", nameof(controlPoints));
            DrawInternal(controlPoints, distance);
        }

        protected abstract void DrawInternal(PointF[] controlPoints, float distance = .5f);

        protected void DrawControlPoints(PointF[] controlPoints)
        {
            foreach (PointF point in controlPoints)
            {
                graphics.DrawPoint(pen, brush, point, 5f);
            }
        }
    }
}
