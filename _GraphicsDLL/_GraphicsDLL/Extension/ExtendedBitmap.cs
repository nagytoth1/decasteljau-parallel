using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace _GraphicsDLL
{
    public static class ExtendedBitmap
    {
        public static bool IsPointOutsideBitmap(this Bitmap bmp,
            int x, int y)
        {
            return x < 0 || x > bmp.Width ||
                   y < 0 || y > bmp.Height;
        }
        public static void SetLine(this Bitmap bmp,
            Color color, float x1, float y1, float x2, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            float length = Math.Abs(dx);
            if (Math.Abs(dy) > length)
                length = Math.Abs(dy);

            float incX = dx / length;
            float incY = dy / length;

            float actX = x1, actY = y1;
            bmp.SetPixel((int)actX, (int)actY, color);
            
            for (int i = 0; i < length; i++)
            {
                actX += incX;
                actY += incY;
                bmp.SetPixel((int)actX, (int)actY, color);
            }
        }
        public static void SetLine(this Bitmap bmp,
            Color color, PointF p1, PointF p2)
        {
            bmp.SetLine(color, p1.X, p1.Y, p2.X, p2.Y);
        }
        public static void SetPolygon(this Bitmap bmp,
           Color color, PointF[] points)
        {
            int i;
            for (i = 0; i < points.Length - 1; i++)
            {
                bmp.SetLine(color, points[i], points[i + 1]);
            }
            bmp.SetLine(color, points[0], points[i]);
        }        
        public static void ResetPolygon(this Bitmap bmp,
           PictureBox pb, PointF[] points)
        {
            int i;
            for (i = 0; i < points.Length - 1; i++)
            {
                bmp.SetLine(pb.BackColor, points[i], points[i + 1]);
            }
            bmp.SetLine(pb.BackColor, points[0], points[i]);
        }

        public static void FillRec4(this Bitmap bmp,
            int x, int y, Color fillColor, Color backColor)
        {
            if (bmp.IsPointOutsideBitmap(x, y) || 
               !bmp.GetPixel(x,y).Equals(backColor)) //ha vizsgált képpont már ki van töltve, akkor nincs dolgunk
                return; //bázisfeltétel
            bmp.SetPixel(x, y, fillColor);
            bmp.FillRec4(x+1, y, fillColor, backColor); //jobbra
            bmp.FillRec4(x-1, y, fillColor, backColor); //balra
            bmp.FillRec4(x, y+1, fillColor, backColor); //felette
            bmp.FillRec4(x, y-1, fillColor, backColor); //alatta
        }
        public static void FillRec8(this Bitmap bmp,
            int x, int y, Color fillColor, Color backColor)
        {
            if (bmp.IsPointOutsideBitmap(x, y) ||
                !bmp.GetPixel(x, y).Equals2(backColor)) //ha vizsgált képpont már ki van töltve, akkor nincs dolgunk
                return; //bázisfeltétel
            bmp.SetPixel(x, y, fillColor);
            //vízszintes - függőleges irányok
            bmp.FillRec8(x + 1, y, fillColor, backColor);
            bmp.FillRec8(x - 1, y, fillColor, backColor);
            bmp.FillRec8(x, y + 1, fillColor, backColor);
            bmp.FillRec8(x, y - 1, fillColor, backColor);
            //átlók
            bmp.FillRec8(x + 1, y + 1, fillColor, backColor); //jobbra fel
            bmp.FillRec8(x + 1, y - 1, fillColor, backColor); //jobbra le
            bmp.FillRec8(x - 1, y + 1, fillColor, backColor); //balra fel
            bmp.FillRec8(x - 1, y - 1, fillColor, backColor); //balra le
        }
        public static void FillStack4(this Bitmap bmp,
            int x, int y, Color fillColor, Color backColor)
        {
            int[] dx = new int[] { 1, 0, -1, 0 }; //amiket x-ekhez kell adni
            int[] dy = new int[] { 0, 1, 0, -1 }; //amiket y-okhoz kell adni
            int newX, newY;
            Stack<Point> points = new Stack<Point>();
            Point p = new Point(x, y);
            points.Push(p);
            while (points.Count > 0)
            {
                if (bmp.IsPointOutsideBitmap(p.X, p.Y)) return;
                bmp.SetPixel(p.X, p.Y, fillColor);
                for (int i = 0; i < dx.Length; i++)
                {
                    newX = p.X + dx[i];
                    newY = p.Y + dy[i];
                    if (bmp.IsPointOutsideBitmap(newX, newY)) break;
                    if (bmp.GetPixel(newX, newY).Equals2(backColor))
                        points.Push(new Point(newX, newY));
                }
                p = points.Pop(); //kiveszi a tetejéről
            }
        }
        public static void FillEdgeFlag(this Bitmap bmp, Color fillColor)
        {
            FillEdgeFlag(bmp, fillColor, Color.Black); //calling the same method with default parameter Color.Black on edges
        }
        public static void ResetFillStack4(this Bitmap bmp, int startX, int startY, PictureBox pb, Color fillColor)
        {
            FillStack4(bmp, startX, startY, pb.BackColor, fillColor); //calling the same method with default parameter Color.Black on edges
        }
        public static void FillEdgeFlag(this Bitmap bmp,
            Color fillColor, Color edgeColor)
        {
            bool inside;
            for (int scanline = 0; scanline < bmp.Height; scanline++) //for each scanline
            {
                inside = false; //when starting each scanline we are believing we are not inside the polygon
                for (int x = 0; x < bmp.Width; x++)
                {
                    HandleEdges(bmp, scanline, edgeColor, ref inside, ref x);
                    if (inside)
                        bmp.SetPixel(x, scanline, fillColor);
                    //else reset pixel to bgcolor - it's unnecessary
                }
            }
        }
        private static byte CountEdgePixels(Bitmap bmp, int x, int scanline, Color edgeColor)
        {
            const byte ANALYZED_EDGEPIXELS = 5;
            byte edgePixels = 0;
            for (int i = 1; i <= ANALYZED_EDGEPIXELS; i++)
            {
                if (bmp.GetPixel(x + i, scanline).Equals2(edgeColor))
                    edgePixels++;
            }
            return edgePixels;
        }
        private static void HandleEdges(Bitmap bmp, int scanline, Color edgeColor, ref bool inside, ref int x)
        {
            byte edgePixels; //we are going to count the neighboring pixels whether they are matching the edgeColor
            if (bmp.GetPixel(x, scanline).Equals2(edgeColor)) //when we reach an edgepoint, check whether there are more in the "neighborhood"
            {
                edgePixels = CountEdgePixels(bmp, x, scanline, edgeColor);
                x += edgePixels;  //skip the edgepixels because we are not gonna set them
                inside = !inside; //negate only when stepping through edgepoints -> from outside we got inside of the polygon
                //that brings new problem at the same time: 
                    //when two edge meets, we skip them and since we negate only once, therefore the pixels that should be outside gets flagged as inside
            }
        }
    }
}
