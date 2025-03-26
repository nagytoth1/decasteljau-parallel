using System.Drawing;

namespace GraphicsDLL
{
    public struct Vector2
    {
        public float x, y;
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }
        //két vektor skaláris szorzata
        public static float operator *(Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.x + v1.y * v2.y;
        }
        public static explicit operator PointF (Vector2 v)
        {
            return new PointF(v.x, v.y);
        }
        public static explicit operator Vector2 (PointF p)
        {
            return new Vector2(p.X, p.Y);
        }
    }
}