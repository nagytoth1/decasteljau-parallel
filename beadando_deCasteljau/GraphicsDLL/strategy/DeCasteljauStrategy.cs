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
        protected readonly object drawLock; // for parallel cases

        protected Graphics graphics;
        private PointF lastResult;

        protected DeCasteljauStrategy(Graphics graphics)
        {
            this.graphics = graphics;
            brush = new SolidBrush(Color.Salmon);
            pen = new Pen(Color.Black, 2f);
            drawLock = new object();
        }

        public void Draw(PointF[] controlPoints, float distance = .5f){
            DrawControlPoints(controlPoints);
            if (controlPoints == null || controlPoints.Length == 0) 
                throw new ArgumentException("Control points should not be null or empty!", nameof(controlPoints));
            PointF result = DrawInternal(controlPoints, distance);
            if (this.lastResult != PointF.Empty)
            {
                graphics.DrawLine(pen, lastResult, result);
            }
            lastResult = result;
            graphics.DrawPoint(pen, brush, result, POINT_SIZE);
        }

        protected abstract PointF DrawInternal(PointF[] controlPoints, float distance = .5f);

        protected void DrawControlPoints(PointF[] controlPoints)
        {
            foreach (PointF point in controlPoints)
            {
                graphics.DrawPoint(pen, Brushes.Magenta, point, 5f);
            }
        }
    }
}
