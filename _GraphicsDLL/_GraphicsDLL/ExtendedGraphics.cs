using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace _GraphicsDLL
{
    public static class ExtendedGraphics
    {
        public static void DrawPixel(this Graphics g, Pen pen, float x, float y)
        {
            g.DrawRectangle(pen, x, y, .5f, .5f);
        }
        #region Line
        public static void DrawLineDDA(this Graphics g, Pen pen,
            float x1, float y1, float x2, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;

            float length = Math.Abs(dx);

            if (Math.Abs(dy) > length)
                length = Math.Abs(dy);

            float actX = x1;
            float actY = y1;
            float incX = dx / length; //ha dx-et választjuk step-nek, akkor ez vagy +1 vagy -1 lesz
            float incY = dy / length; //felbontjuk step db szakaszra

            g.DrawPixel(pen, actX, actY); //legelső pontot gyújtsuk ki
            for (int i = 0; i < length; i++)
            {
                actX += incX;
                actY += incY;
                g.DrawPixel(pen, actX, actY);
            }
        }
        public static void DrawLineDDA(this Graphics g, Pen pen,
            int x1, int y1, int x2, int y2)
        {
            g.DrawLineDDA(pen, (float)x1, (float)y1, (float)x2, (float)y2);
        }
        public static void DrawLineDDA(this Graphics g, Pen pen,
            PointF p1, PointF p2)
        {
            g.DrawLineDDA(pen, p1.X, p1.Y, p2.X, p2.Y);
        }
        public static void DrawLineDDA(this Graphics g, Pen pen,
            Point p1, Point p2)
        {
            g.DrawLineDDA(pen, p1.X, p1.Y, p2.X, p2.Y);
        }
        #region Color Transition
        public static void DrawLineDDA(this Graphics g, Color c1, Color c2,
            float x1, float y1, float x2, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;

            float length = Math.Abs(dx);
            if (Math.Abs(dy) > length)
                length = Math.Abs(dy);

            float actX = x1, actY = y1;
            float incX = dx / length;
            float incY = dy / length;

            float actR = c1.R, actG = c1.G, actB = c1.B;
            float incR = (c2.R - c1.R) / length,
                  incG = (c2.G - c1.G) / length,
                  incB = (c2.B - c1.B) / length;

            Pen pen = new Pen(c1, 3f);
            for (int i = 0; i < length; i++)
            {
                actX += incX; actY += incY;
                g.DrawPixel(pen, actX, actY);
                actR += incR; actG += incG; actB += incB;
                pen.Color = Color.FromArgb(
                    (byte)Math.Round(actR, 0),
                    (byte)Math.Round(actG, 0),
                    (byte)Math.Round(actB, 0));
            }
            pen.Color = Color.FromArgb((byte)Math.Round(actR, 0),
                    (byte)Math.Round(actG, 0),
                    (byte)Math.Round(actB, 0));
            g.DrawPixel(pen, actX, actY);
        }
        public static void DrawLineDDA(this Graphics g, Color c1, Color c2,
            int x1, int y1, int x2, int y2)
        {
            g.DrawLineDDA(c1, c2, (float)x1, (float)y1, (float)x2, (float)y2);
        }
        public static void DrawLineDDA(this Graphics g, Color c1, Color c2,
            PointF p1, PointF p2)
        {
            g.DrawLineDDA(c1, c2, p1.X, p1.Y, p2.X, p2.Y);
        }
        public static void DrawLineDDA(this Graphics g, Color c1, Color c2,
            Point p1, Point p2)
        {
            g.DrawLineDDA(c1, c2, p1.X, p1.Y, p2.X, p2.Y);
        }
        #endregion
        #endregion
        #region LineMidpoint
        public static void DrawLineMidPoint(this Graphics g, Pen pen, 
            int x1, int y1, int x2, int y2)
        {
            //minden pixelnek egész koordinátái vannak
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            //melyik van kisebb távolságra az egyenestől
            //egyenes x-tengellyel bezárt szöge alfa: 
                // 0 <= m < 1 és m = tan alfa, ezért 0 <= alfa < 45 -> dx > dy
            //implicit egyenlet: F(x, y) = 0 vagy F(x, y) = c alakban adható meg
            //paraméteres: r(t) = p + t * v
            //F(x,y) = Ax + By + C = 0, ahol A = dy, B = -d_x, C = bd_x
            //ha F(x,y) = 0-t kapunk, akkor rajta van az egyenesen
            //minden x szerinti lépéskor kiszámítjuk F(x_p+1;y_p+1) értékét -> lassú lesz az algo
            int sign_dx = Math.Sign(x2- x1);
            int sign_dy = Math.Sign(y2- y1);
            int r, d, actX, actY;
            bool t;
            if (dx < dy)
            {
                r = dx;
                dx = dy;
                dy = r;
                t = true;
            }
            else t = false;

            d = 2 * dy - dx;
            actX = x1;
            actY = y1;
            while ((actX != x2) || (actY != y2))
            {
                if(d > 0)
                {
                    if (t)
                        actX += sign_dx;
                    else
                        actY += sign_dy;
                    d = d - 2 * dx;
                }

                if (t)
                    actY += sign_dy;
                else
                    actX += sign_dx;

                d = d + 2 * dy;
                g.DrawPixel(pen, actX, actY);
            }
        }
        public static void DrawLineMidPoint(this Graphics g, Pen pen,
            float x1, float y1, float x2, float y2)
        {
            g.DrawLineMidPoint(pen,
                (int)Math.Round(x1),
                (int)Math.Round(y1),
                (int)Math.Round(x2),
                (int)Math.Round(y2));
        }
        public static void DrawLineMidPoint(this Graphics g, Pen pen,
        Point p1, Point p2)
        {
            g.DrawLineMidPoint(pen,
                p1.X, p1.Y, p2.X, p2.Y);
        }
        public static void DrawLineMidPoint(this Graphics g, Pen pen,
        PointF p1, PointF p2)
        {
            g.DrawLineMidPoint(pen,
                p1.X, p1.Y, p2.X, p2.Y);
        }
        #endregion
        #region Point
        public static void DrawPoint(this Graphics g, Pen pen, Brush brush,
            PointF p, float r)
        {
            g.FillEllipse(brush, p.X - r, p.Y - r, 2 * r, 2 * r);
            g.DrawEllipse(pen, p.X - r, p.Y - r, 2*r, 2*r);
        }
        #endregion
        #region Polygon
        public static void DrawPolygonDDA(this Graphics g, Pen pen, PointF[] points, bool dontknow)
        {
            int i;
            for (i = 0; i < points.Length - 1; i++)
            {
                g.DrawLineDDA(pen, points[i], points[i + 1]);
            }
            g.DrawLineDDA(pen, points[0], points[i]);
        }
        #endregion
        #region Circle
        public static void DrawCirclePixels(this Graphics g, Pen pen,
            float cx, float cy, float x, float y)
        {
            //egyszerre 8 pontot tudunk kirajzolni
            g.DrawPixel(pen, cx + x, cy + y);
            g.DrawPixel(pen, cx + x, cy - y);
            g.DrawPixel(pen, cx - x, cy + y);
            g.DrawPixel(pen, cx - x, cy - y);

            g.DrawPixel(pen, cx + y, cy + x);
            g.DrawPixel(pen, cx + y, cy - x);
            g.DrawPixel(pen, cx - y, cy + x);
            g.DrawPixel(pen, cx - y, cy - x);
        }
        //Midpoint kör + színátmenet / színes kör
        public static void DrawCircle(this Graphics g, Pen pen, 
            PointF center, float r)
        {
            float x = 0, 
                  y = r, 
                  h = 1f - r; //h: döntési változó

            while (y > x)
            {
                if (h < 0)
                    h += 2 * x + 3;
                else
                {
                    h += 2 * (x - y) + 5;
                    y--;
                }
                x++;
                g.DrawCirclePixels(pen, center.X, center.Y, x, y);
            }
        }
        public static void DrawCircle(this Graphics g, Color c1, Color c2,
            PointF center, float r)
        {
            float x = 0,
                  y = r,
                  h = 1f - r; //h: döntési változó
            float incR = (c2.R - c1.R) / y,
                  incG = (c2.G - c1.G) / y,
                  incB = (c2.B - c1.B) / y;
            float actR = c1.R, 
                actG = c1.G, 
                actB = c1.B;
            Pen pen = new Pen(c1, 1f);
            g.DrawCirclePixels(pen, center.X, center.Y, x, y);
            while (y > x)
            {
                if (h < 0)
                    h = h + 2 * x + 3;
                else
                {
                    h = h + 2*x - 2*y + 5;
                    y--;
                }
                x++;
                actR += incR; actG += incG; actB += incB;
                pen.Color = Color.FromArgb(
                    (byte) Math.Round(actR),
                    (byte)Math.Round(actG),
                    (byte)Math.Round(actB));
                g.DrawCirclePixels(pen, center.X, center.Y, x, y);
            }
        }
        #endregion
        #region CircleMidpoint

        #endregion
        #region CohenSutherland
        //4 bites kód: TBLR
        private const byte TOP = 0b_0000_1000;
        private const byte BOT = 0b_0000_0100;
        private const byte LEFT = 0b_0000_0010;
        private const byte RIGHT = 0b_0000_0001;

        private static byte OuterCode(Rectangle window, PointF p)
        {
            byte code = 0b_0000_0000; //vegyük azt, hogy a pont a képernyőn belül van
            if (p.X < window.Left)
                code |= LEFT;
            else if (p.X > window.Right) //egyszerre nem következhet be 1. klózzal
                code |= RIGHT;
            if (p.Y < window.Top)
                code |= TOP;
            else if (p.Y > window.Bottom) //egyszerre nem következhet be 3. klózzal
                code |= BOT;
            return code;
        }

        public static void Clip(this Graphics g, Pen pen, 
            Rectangle window, PointF p1, PointF p2)
        {
            float newX = 0f, newY = 0f;
            byte code1 = OuterCode(window, p1);
            byte code2 = OuterCode(window, p2);
            byte code_out; //annak a kódja, amelyik kint van
            bool accept = false;
            while (true)
            {
                if ((code1 | code2) == 0) //ha mindkét végpont a képernyőn belül van, akkor elfogadjuk az egyenest kirajzolásra
                {
                    accept = true;
                    break;
                }
                //amikor kint van egyik oldalon, elutasítom
                if ((code1 & code2) != 0)
                    break;
                code_out = code1 != 0 ? 
                    code1 : 
                    code2; //melyik a nemnulla, melyik van kint?
                if((code_out & LEFT) != 0) //balra lóg ki
                {
                    newX = window.Left;
                    newY = p1.Y + (p2.Y - p1.Y) * (window.Left - p1.X) / (p2.X - p1.X);
                }
                else if((code_out & RIGHT) != 0) //jobbra lóg ki
                {
                    newX = window.Right;
                    newY = p1.Y + (p2.Y - p1.Y) * (window.Right - p1.X) / (p2.X - p1.X);
                }
                else if ((code_out & TOP) != 0) //fentre lóg ki
                {
                    newY = window.Top;
                    newX = p1.X + (p2.X - p1.X) * (window.Top - p1.Y) / (p2.Y - p1.Y);
                }
                else
                {
                    newY = window.Bottom;
                    newX = p1.X + (p2.X - p1.X) * (window.Bottom - p1.Y) / (p2.Y - p1.Y);
                }
                if (code1 != 0)
                {
                    p1.X = newX;
                    p1.Y = newY;
                    code1 = OuterCode(window, p1);
                }
                else
                {
                    p2.X = newX;
                    p2.Y = newY;
                    code2 = OuterCode(window, p2);
                }
            }
            
            if (accept)
                g.DrawLine(pen, p1, p2);
        }
        #endregion
        public static void DrawParametricCurve2D(this Graphics g, Pen pen, 
            Func<float, float> X, Func<float, float> Y,
            float a_limit, float b_limit, float scale = 1.0f, 
            float centerX = 0, float centerY = 0, ushort numberOfPoints = 500) //hány pontot szeretnénk kirajzolni
        {
            if (a_limit >= b_limit)
                throw new Exception("Interval's second limit must be greater than the first one.");
            float actPoint = a_limit; //a-tól indulunk, aktuális osztópont lesz
            float length = (b_limit - a_limit) / numberOfPoints; //lépésköz
            PointF prevPoint = new PointF(
                x: scale * X(actPoint) + centerX,
                y: scale * Y(actPoint) + centerY); //tehát igazából (0,0) középpontnál semmit nem adunk hozzá, 1-es szorzónál eredeti méretben látjuk
            PointF nextPoint;
            while (actPoint < b_limit) //b-ig megyünk
            {
                actPoint += length;
                nextPoint = new PointF(scale * X(actPoint) + centerX,
                                       scale * Y(actPoint) + centerY);
                g.DrawLine(pen, prevPoint, nextPoint); //közelítő szakaszt húzunk
                prevPoint = nextPoint;
            }
        }
        #region BezierCurve
        public static void DrawBezier3(this Graphics g, Pen pen,
            Bezier3Curve curve)
        {
            g.DrawParametricCurve2D(pen,
                t => (float) (Bezier3Curve.B0(t) * curve.p0.x + Bezier3Curve.B1(t) * curve.p1.x +
                              Bezier3Curve.B2(t) * curve.p2.x + Bezier3Curve.B3(t) * curve.p3.x),
                t => (float) (Bezier3Curve.B0(t) * curve.p0.x + Bezier3Curve.B1(t) * curve.p1.x +
                              Bezier3Curve.B2(t) * curve.p2.x + Bezier3Curve.B3(t) * curve.p3.x),
                0f, 1f);
        }
        #endregion
    }
}
